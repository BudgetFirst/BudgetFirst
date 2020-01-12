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

namespace BudgetFirst.Budgeting.Domain.Events
{
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Domain.Events;
    
    /// <summary>
    /// A new budget has been added
    /// </summary>
    [DataContract(Name = "AddedBudget", Namespace = "http://budgetfirst.github.io/schemas/2016/08/15/Budgeting/AddedBudget")]
    [ComVisible(false)]
    public class AddedBudget : DomainEvent<BudgetId>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AddedBudget"/> class. 
        /// </summary>
        /// <param name="name">Name of the budget</param>
        /// <param name="currencyCode">Currency code</param>
        /// <remarks>Will not be called during de-serialisation</remarks>
        public AddedBudget(string name, string currencyCode)
        {
            this.Name = name;
            this.CurrencyCode = currencyCode;
        }

        /// <summary>
        /// Gets the budget id
        /// </summary>
        public BudgetId BudgetId => this.AggregateId;

        /// <summary>
        /// Gets the name of the budget
        /// </summary>
        [DataMember(Name = "Name")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the currency code used in the budget
        /// </summary>
        /// <remarks>TODO: replace with separate type?</remarks>
        [DataMember(Name = "CurrencyCode")]
        public string CurrencyCode { get; private set; } 
    }
}