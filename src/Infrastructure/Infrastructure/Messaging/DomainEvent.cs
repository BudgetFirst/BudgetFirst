﻿// BudgetFirst 
// ©2016 Thomas Mühlgrabner
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

namespace BudgetFirst.Infrastructure.Messaging
{
    using System;
    using System.Runtime.Serialization;

    using BudgetFirst.Infrastructure.Persistency;

    /// <summary>
    /// An event, which is raised by an aggregate
    /// </summary>
    [DataContract(Name = "DomainEvent", Namespace = "http://budgetfirst.github.io/schemas/2016/04/23/DomainEvent")]
    public abstract class DomainEvent : IComparable
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DomainEvent"/> class. 
        /// Initialises the EventId and Timestamp.
        /// </summary>
        /// <remarks>Should not be called during de-serialisation</remarks>
        public DomainEvent()
        {
            this.EventId = Guid.NewGuid();
            this.Timestamp = DateTime.UtcNow;
            /* Device id, aggregate id and vector clock should be set by sender */
        }

        /// <summary>
        /// Gets or sets the Id of the event
        /// </summary>
        [DataMember(Name = "EventId")]
        public Guid EventId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the device that the event happened on
        /// </summary>
        [DataMember(Name = "readOnlyDeviceId")]
        public Guid DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the aggregate that this event was raised by
        /// </summary>
        [DataMember(Name = "AggregateId")]
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp of when the event occurred
        /// </summary>
        [DataMember(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the VectorClock for the event
        /// </summary>
        [DataMember(Name = "VectorClock")]
        public VectorClock VectorClock { get; set; }
        
        /// <summary>
        /// Is this event valid or are there missing fields?
        /// </summary>
        /// <returns><c>true</c> if all required fields are set</returns>
        public bool IsValid()
        {
            return this.DeviceId != Guid.Empty && this.AggregateId != Guid.Empty && this.VectorClock != null;
        }

        /// <summary>
        /// Compares this Event with a second event and determines the order 
        /// they happened, based on the VectorClock.
        /// </summary>
        /// <param name="obj">The event to compare</param>
        /// <returns>1 if this event happened after, -1 if this event happened before, 0 if the order can not be determined</returns>
        public int CompareTo(object obj)
        {
            var event2 = obj as DomainEvent;
            if (event2 == null)
            {
                return 0;
            }

            var result = this.VectorClock.CompareTo(event2.VectorClock);
            if (result == 0)
            {
                // ensure an absolute order if the order cannot be determined. Fallback to device id
                // This should not result in 0 because the vector clock must be different for the same device id
                result = this.DeviceId.CompareTo(event2.DeviceId);
            }

            return result;
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        /// <param name="obj">Other event</param>
        /// <returns><c>true</c> if the other event is the same as this event</returns>
        public override bool Equals(object obj)
        {
            var other = obj as DomainEvent;
            if (other == null)
            {
                return false;
            }

            // Don't compare timestamps, I'm not sure if that would be very reliable (I had issues with timestamps and roundtrips in the past)
            return this.EventId == other.EventId && this.AggregateId == other.AggregateId
                   && this.DeviceId == other.DeviceId && this.VectorClock == other.VectorClock;
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
    }
}
