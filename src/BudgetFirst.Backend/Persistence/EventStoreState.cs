﻿// BudgetFirst 
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

namespace BudgetFirst.Backend.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
        
    /// <summary>
    /// State for the <see cref="EventStore"/>
    /// </summary>
    [DataContract(Name = "EventStoreState", Namespace = "http://budgetfirst.github.io/schemas/2020/02/08/EventStoreState")]
    public sealed class EventStoreState
    {
        /// <summary>
        /// Contains the list of events in this store
        /// </summary>
        [DataMember(Name = "Events")]
        private List<Event> events;

        /// <summary>
        /// Gets or sets the events in this store.
        /// Is guaranteed to be not <c>null</c>.
        /// </summary>
        public List<Event> Events
        {
            get
            {
                return this.events ?? (this.events = new List<Event>());
            }

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.events = value;
            }
        }
    }
}