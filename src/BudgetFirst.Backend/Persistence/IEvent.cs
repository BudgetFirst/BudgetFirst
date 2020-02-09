// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
// This source code is licensed under:
//   * GNU General Public License v3.0 (GPLv3)
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

// TODO: not sure if we even need an interface. We could just go with the base class.

namespace BudgetFirst.Backend.EventStore
{
    using System;

    /// <summary>
    /// Base interface for events
    /// </summary>
    public interface IEvent
    {
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