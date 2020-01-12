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

namespace BudgetFirst.Common.Infrastructure.Domain.Model
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// Base class for aggregate Id value objects
    /// </summary>
    [DataContract(Name = "AggregateId", Namespace = "http://budgetfirst.github.io/schemas/2016/08/15/Identifiers/AggregateId")]
    [ComVisible(false)]
    public abstract class AggregateId
    {
        /// <summary>
        /// Internal id
        /// </summary>
        [DataMember(Name = "Id")]
        private Guid id;

        /// <summary>
        /// Initialises a new instance of the <see cref="AggregateId"/> class.
        /// </summary>
        /// <param name="id">Underlying guid</param>
        public AggregateId(Guid id)
        {
            this.id = id;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((AggregateId)obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        /// <summary>
        /// Create a <see cref="Guid"/> from this id.
        /// </summary>
        /// <returns>Id in guid form</returns>
        public Guid ToGuid()
        {
            return this.id;
        }

        /// <summary>
        /// Is this object equal to another?
        /// </summary>
        /// <param name="other">Other object</param>
        /// <returns>Value indicating if the other object is considered to be equal</returns>
        protected bool Equals(AggregateId other)
        {
            return this.id.Equals(other.id);
        }
    }
}