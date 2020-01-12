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

    using BudgetFirst.Common.Infrastructure.Domain.Events;

    /// <summary>
    /// Represents a message bus/queue, publish-subscribe pattern.
    /// Subscribers are to be automatically resolved
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publish an event to all subscribers of that event
        /// </summary>
        /// <typeparam name="TDomainEvent">Event type</typeparam>
        /// <param name="domainEvent">Event to publish</param>
        void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent;

        /// <summary>
        /// Register as a subscriber
        /// </summary>
        /// <typeparam name="TDomainEvent">Type of event</typeparam>
        /// <param name="handler">Event handler</param>
        void Subscribe<TDomainEvent>(Action<TDomainEvent> handler) where TDomainEvent : class, IDomainEvent;
    }
}
