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
namespace BudgetFirst.Common.Domain.Model.Identifiers
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    using BudgetFirst.Common.Infrastructure.Domain.Model;

    /// <summary>
    /// Budget id
    /// </summary>
    [ComVisible(false)]
    [DataContract(Name = "BudgetId", Namespace = "http://budgetfirst.github.io/schemas/2016/08/15/Identifiers/BudgetId")
    ]
    public sealed class BudgetId : AggregateId
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BudgetId"/> class.
        /// </summary>
        /// <param name="id">Underlying id</param>
        public BudgetId(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Gets the off-budget id
        /// </summary>
        public static BudgetId OffBudgetId
        {
            get
            {
                return new BudgetId(Guid.Empty);
            }
        }

        /// <summary>
        /// Is this for the off-budget "budget"?
        /// </summary>
        /// <returns><c>true</c> if this identifies the off-budget budget</returns>
        public bool IsOffBudget()
        {
            return this.Equals(OffBudgetId);
        }
    }
}