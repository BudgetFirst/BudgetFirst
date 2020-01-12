// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
//
// This source code is dual-licensed under:
//   * Mozilla Public License 2.0 (MPL 2.0) 
//   * GNU General Public License v3.0 (GPLv3)
//
// ==================== Mozilla Public License 2.0 ===================
// This Source Code Form is subject to the terms of the Mozilla Public 
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// ================= GNU General Public License v3.0 =================
// This file is part of BudgetFirst.
//
// BudgetFirst is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BudgetFirst is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Budget First.  If not, see<http://www.gnu.org/licenses/>.
// ===================================================================

namespace BudgetFirst.Common.Infrastructure.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    using BudgetFirst.Common.Infrastructure.Domain.Events;
    using BudgetFirst.Common.Infrastructure.Messaging;
    using BudgetFirst.Common.Infrastructure.Persistency;

    /// <summary>
    /// Represents an aggregate root
    /// </summary>
    /// <typeparam name="TIdentifier">Aggregate id type</typeparam>
    [ComVisible(false)]
    public abstract class AggregateRoot<TIdentifier> where TIdentifier : AggregateId
    {
        /// <summary>
        /// Aggregate Id
        /// </summary>
        private readonly TIdentifier aggregateId;

        /// <summary>
        /// Event handlers
        /// </summary>
        private readonly Dictionary<Type, Action<DomainEvent<TIdentifier>>> eventHandlers;

        /// <summary>
        /// Current unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initialises a new instance of the <see cref="AggregateRoot{TIdentifier}"/> class.
        /// Can be used to represent rehydrated as well as new aggregates. Each aggregate must have a unique Id.
        /// </summary>
        /// <param name="id">Id of the aggregate</param>
        /// <param name="unitOfWork">Current unit of work</param>
        protected AggregateRoot(TIdentifier id, IUnitOfWork unitOfWork)
        {
            this.aggregateId = id;
            this.eventHandlers = new Dictionary<Type, Action<DomainEvent<TIdentifier>>>();
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Aggregate Id
        /// </summary>
        public TIdentifier Id => this.aggregateId;

        /// <summary>
        /// Define an event handler
        /// </summary>
        /// <typeparam name="TDomainEvent">Type of event to handle</typeparam>
        /// <param name="handler">Event handler for the event</param>
        protected void Handles<TDomainEvent>(Action<TDomainEvent> handler) where TDomainEvent : DomainEvent<TIdentifier>
        {
            this.eventHandlers[typeof(TDomainEvent)] = @event => handler.Invoke((TDomainEvent)@event);
        }

        /// <summary>
        /// Load the state from history
        /// </summary>
        /// <param name="domainEvents">List of domain events</param>
        protected void LoadFrom(IEnumerable<DomainEvent<TIdentifier>> domainEvents)
        {
            foreach (var domainEvent in domainEvents.Where(aggregate => this.aggregateId.Equals(aggregate.AggregateId)))
            {
                this.HandleEvent(domainEvent);
            }
        }

        /// <summary>
        /// Apply event
        /// </summary>
        /// <typeparam name="TDomainEvent">Type of the event to raise (and handle)</typeparam>
        /// <param name="domainEvent">Event to raise and handle</param>
        protected void Apply<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent<TIdentifier>
        {
            domainEvent.AggregateId = this.aggregateId;
            domainEvent.DeviceId = this.unitOfWork.ReadOnlyDeviceId.GetDeviceId();
            this.ApplyVectorClock(domainEvent);
            this.HandleEvent(domainEvent);
            this.unitOfWork.NewEvents.Add(domainEvent);
        }

        /// <summary>
        /// Apply the current vector clock to the event (and increment it)
        /// </summary>
        /// <typeparam name="TDomainEvent">Type of the event to raise (and handle)</typeparam>
        /// <param name="domainEvent">Event to raise and handle</param>
        private void ApplyVectorClock<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent<TIdentifier>
        {
            this.unitOfWork.VectorClock.Increment();
            domainEvent.VectorClock = this.unitOfWork.VectorClock.GetVectorClock();
        }

        /// <summary>
        /// Handles a specific event
        /// </summary>
        /// <typeparam name="TDomainEvent">Type of the event to handle</typeparam>
        /// <param name="domainEvent">Event to handle</param>
        private void HandleEvent<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent<TIdentifier>
        {
            // Assumption: an aggregate must always be able to handle all events that it raised in the past.
            var handlingAction = this.eventHandlers[domainEvent.GetType()];
            handlingAction.Invoke(domainEvent);
        }
    }
}
