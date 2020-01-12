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

namespace BudgetFirst.Common.Infrastructure.Persistency
{
    using System;
    using System.Collections.Generic;

    using BudgetFirst.Common.Infrastructure.ApplicationState;
    using BudgetFirst.Common.Infrastructure.Domain.Events;
    using BudgetFirst.Common.Infrastructure.Domain.Model;

    /// <summary>
    /// Unit of work for aggregates.
    /// Should only be used on a single aggregate per unit of work.
    /// Combine multiple unit of works for multiple aggregates in a saga.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the device Id
        /// </summary>
        IReadOnlyDeviceId ReadOnlyDeviceId { get; }

        /// <summary>
        /// Gets the vector clock
        /// </summary>
        IVectorClock VectorClock { get; }

        /// <summary>
        /// Gets the list of new events in this unit of work (not all events from the event store!)
        /// </summary>
        IList<IDomainEvent> NewEvents { get; }

        /// <summary>
        /// Get ALL events for the aggregate - includes new events from the unit of work and events from the event store.
        /// </summary>
        /// <param name="aggregateId">Aggregate id</param>
        /// <typeparam name="TAggregateId">Aggregate id type</typeparam>
        /// <returns>All events (from store and unit of work) for the aggregate</returns>
        IReadOnlyList<DomainEvent<TAggregateId>> GetEventsForAggregate<TAggregateId>(TAggregateId aggregateId)
            where TAggregateId : AggregateId;
    }
}
