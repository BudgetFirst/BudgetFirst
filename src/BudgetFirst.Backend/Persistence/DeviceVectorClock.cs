// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
//
// This source code is licensed under:
//   * GNU General Public License v3.0 (GPLv3)
//
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

namespace BudgetFirst.Backend.EventStore
{
    using System;

    /// <summary>
    /// A vector clock tied to a device
    /// </summary>
    public class DeviceVectorClock
    {
        /// <summary>
        /// Current vector clock
        /// </summary>
        private VectorClock vectorClock;

        /// <summary>
        /// Current device Id
        /// </summary>
        private DeviceId deviceId;

        /// <summary>
        /// Initialises a new instance of the <see cref="DeviceVectorClock"/> class.
        /// </summary>
        /// <param name="deviceId">Device id</param>
        public DeviceVectorClock(DeviceId deviceId)
        {
            this.deviceId = deviceId;
            this.vectorClock = new VectorClock();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DeviceVectorClock"/> class.
        /// </summary>
        /// <param name="deviceId">Device id</param>
        /// <param name="vectorClock">Current vector clock</param>
        private DeviceVectorClock(DeviceId deviceId, VectorClock vectorClock)
        {
            this.deviceId = deviceId;
            this.vectorClock = vectorClock.Copy();
        }

        /// <summary>
        /// Set the current state
        /// </summary>
        /// <param name="vectorClock">Vector clock</param>
        public void SetState(VectorClock vectorClock)
        {
            if(vectorClock is null)
            {
                throw new ArgumentNullException(nameof(vectorClock));
            }
            this.vectorClock = vectorClock.Copy();
        }

        /// <summary>
        /// Increment the current vector clock
        /// </summary>
        public void Increment()
        {
            var newValue = this.vectorClock.Increment(this.deviceId.ToString()); // use device id specific tostring. 
            this.vectorClock = newValue;
        }

        /// <summary>
        /// Create a copy of this vector clock
        /// </summary>
        /// <returns>A clone of this vector clock</returns>
        public DeviceVectorClock Clone()
        {
            return new DeviceVectorClock(this.deviceId, this.vectorClock);
        }

        /// <summary>
        /// Get the current vector clock
        /// </summary>
        /// <returns>Underlying vector clock</returns>
        public VectorClock GetVectorClock()
        {
            return this.vectorClock.Copy();
        }

        /// <summary>
        /// Update the vector clock to a new value
        /// </summary>
        /// <param name="value">New value to set</param>
        public void Update(VectorClock value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            this.vectorClock = value.Copy();
        }
    }
}