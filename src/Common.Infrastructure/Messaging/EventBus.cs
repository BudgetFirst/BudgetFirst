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

namespace BudgetFirst.Common.Infrastructure.Messaging
{
    using System;
    using System.Collections.Generic;

    using BudgetFirst.Common.Infrastructure.Domain.Events;

    /// <summary>
    /// A publish/subscribe message bus for events
    /// </summary>
    public class EventBus : IEventBus
    {
        /// <summary>
        /// Registered subscribers
        /// </summary>
        private Dictionary<Type, List<Action<IDomainEvent>>> registrations; // code smell

        /// <summary>
        /// Initialises a new instance of the <see cref="EventBus"/> class.
        /// </summary>
        public EventBus()
        {
            this.registrations = new Dictionary<Type, List<Action<IDomainEvent>>>();
        }

        /// <summary>
        /// Publish an event to all subscribers
        /// </summary>
        /// <typeparam name="TDomainEvent">Type of the event to publish</typeparam>
        /// <param name="domainEvent">Event to publish</param>
        public void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent
        {
            List<Action<IDomainEvent>> handlers;
            this.registrations.TryGetValue(domainEvent.GetType(), out handlers);
            if (handlers == null)
            {
                return;
            }

            foreach (var subscriber in handlers)
            {
                subscriber.Invoke(domainEvent);
            }
        }

        /// <summary>
        /// Subscribe to events of a specific type
        /// </summary>
        /// <typeparam name="TDomainEvent">Event type</typeparam>
        /// <param name="handler">Event handler to register</param>
        public void Subscribe<TDomainEvent>(Action<TDomainEvent> handler) where TDomainEvent : class, IDomainEvent
        {
            var eventType = typeof(TDomainEvent);
            if (!this.registrations.ContainsKey(eventType))
            {
                this.registrations[eventType] = new List<Action<IDomainEvent>>();
            }

            this.registrations[eventType].Add(@event => handler.Invoke((TDomainEvent)@event));
        }
    }
}
