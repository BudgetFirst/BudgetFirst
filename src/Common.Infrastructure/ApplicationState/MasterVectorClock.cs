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

namespace BudgetFirst.Common.Infrastructure.ApplicationState
{
    using BudgetFirst.Common.Infrastructure.Domain.Events;
    using BudgetFirst.Common.Infrastructure.Messaging;

    /// <summary>
    /// Represents the master vector clock
    /// </summary>
    public class MasterVectorClock : IVectorClock
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
        /// Initialises a new instance of the <see cref="MasterVectorClock"/> class.
        /// </summary>
        /// <param name="deviceId">Device id</param>
        public MasterVectorClock(DeviceId deviceId)
        {
            this.deviceId = deviceId;
            this.vectorClock = new VectorClock();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MasterVectorClock"/> class.
        /// </summary>
        /// <param name="deviceId">Device id</param>
        /// <param name="vectorClock">Current vector clock</param>
        private MasterVectorClock(DeviceId deviceId, VectorClock vectorClock)
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
            this.vectorClock = vectorClock.Copy();
        }

        /// <summary>
        /// Increment the current vector clock
        /// </summary>
        public void Increment()
        {
            var newValue = this.vectorClock.Increment(this.deviceId.ToString()); // use device id specific tostring
            this.vectorClock = newValue;
        }

        /// <summary>
        /// Create a copy of this vector clock
        /// </summary>
        /// <returns>A clone of this vector clock</returns>
        public IVectorClock Clone()
        {
            return new MasterVectorClock(this.deviceId, this.vectorClock);
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
        public void Update(IVectorClock value)
        {
            this.vectorClock = value.GetVectorClock().Copy();
        }
    }
}