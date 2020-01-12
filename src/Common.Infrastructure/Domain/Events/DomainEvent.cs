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
    using System.Runtime.Serialization;

    using BudgetFirst.Common.Infrastructure.Domain.Model;

    /// <summary>
    /// An event, which is raised by an aggregate
    /// </summary>
    /// <typeparam name="TAggregateId">Aggregate id type</typeparam>
    [DataContract(Name = "DomainEvent", Namespace = "http://budgetfirst.github.io/schemas/2016/04/23/DomainEvent")]
    public abstract class DomainEvent<TAggregateId> : AbstractDomainEvent, IDomainEvent
        where TAggregateId : AggregateId
    {
        /// <summary>
        /// Gets or sets the aggregate id
        /// </summary>
        public TAggregateId AggregateId
        {
            get
            {
                return (TAggregateId)this.AbstractAggregateId;
            }

            set
            {
                this.AbstractAggregateId = value;
            }
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        /// <param name="obj">Other event</param>
        /// <returns><c>true</c> if the other event is the same as this event</returns>
        public override bool Equals(object obj)
        {
            var other = obj as DomainEvent<TAggregateId>;
            if (other == null)
            {
                return false;
            }

            // Don't compare timestamps, I'm not sure if that would be very reliable (I had issues with timestamps and roundtrips in the past)
            return this.EventId == other.EventId && this.AggregateId.Equals(other.AggregateId)
                   && this.DeviceId == other.DeviceId && this.VectorClock.Equals(other.VectorClock);
        }

        /// <summary>
        /// Get the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.EventId.GetHashCode();
                hashCode = (hashCode * 397) ^ this.DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ this.AggregateId.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.VectorClock != null ? this.VectorClock.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        /// Is this event valid or are there missing fields?
        /// </summary>
        /// <returns><c>true</c> if all required fields are set</returns>
        public bool IsValid()
        {
            return this.DeviceId != Guid.Empty && this.AggregateId != null && this.VectorClock != null;
        }
    }
}