// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
// This source code is dual-licensed under:
//   * Mozilla Public License 2.0 (MPL 2.0) 
//   * GNU General Public License v3.0 (GPLv3)
// ==================== Mozilla Public License 2.0 ===================
// This Source Code Form is subject to the terms of the Mozilla Public 
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// ================= GNU General Public License v3.0 =================
// This file is part of BudgetFirst.
// BudgetFirst is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// BudgetFirst is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Budget First.  If not, see<http://www.gnu.org/licenses/>.
// ===================================================================
namespace BudgetFirst.Common.Infrastructure.Domain.Events
{
    using System;

    using BudgetFirst.Common.Infrastructure.Domain.Model;

    /// <summary>
    /// Base interface for domain events
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the (base type) aggregate id
        /// </summary>
        AggregateId AbstractAggregateId { get; }

        /// <summary>
        /// Gets the Id of the device that the event happened on
        /// </summary>
        Guid DeviceId { get; }

        /// <summary>
        /// Gets the Id of the event
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// Gets the UTC timestamp of when the event occurred
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the VectorClock for the event
        /// </summary>
        VectorClock VectorClock { get; }

        /// <summary>
        /// Is this event valid or are there missing fields?
        /// </summary>
        /// <returns><c>true</c> if all required fields are set</returns>
        bool IsValid();
    }
}