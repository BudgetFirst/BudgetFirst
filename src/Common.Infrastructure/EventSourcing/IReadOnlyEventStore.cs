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

namespace BudgetFirst.Common.Infrastructure.EventSourcing
{
    using System;
    using System.Collections.Generic;

    using BudgetFirst.Common.Infrastructure.Domain.Events;
    using BudgetFirst.Common.Infrastructure.Domain.Model;
    using BudgetFirst.Common.Infrastructure.Messaging;

    /// <summary>
    /// Read-only access to the event store
    /// </summary>
    public interface IReadOnlyEventStore
    {
        /// <summary>
        /// Get all events in the store
        /// </summary>
        /// <returns>All events in the store</returns>
        IReadOnlyList<IDomainEvent> GetEvents();

        /// <summary>
        /// Get all saved events for a specific aggregate.
        /// Beware: events are referenced directly, do not manipulate them.
        /// </summary>
        /// <param name="aggregateId">Aggregate Id</param>
        /// <typeparam name="TAggregateId">Aggregate id type</typeparam>
        /// <returns>Reference to all events for the given aggregate</returns>
        IReadOnlyList<DomainEvent<TAggregateId>> GetEventsFor<TAggregateId>(TAggregateId aggregateId) where TAggregateId : AggregateId;
    }
}