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

namespace BudgetFirst.Accounting.Domain.Events
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Domain.Events;

    /// <summary>
    /// A new account was created
    /// </summary>
    [DataContract(Name = "AddedAccount", Namespace = "http://budgetfirst.github.io/schemas/2016/08/15/Accounting/AddedAccount")]
    [ComVisible(false)]
    public class AddedAccount : DomainEvent<AccountId>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AddedAccount"/> class.
        /// </summary>
        /// <param name="name">Account name</param>
        /// <param name="budgetId">Budget the account belongs to</param>
        public AddedAccount(string name, BudgetId budgetId)
        {
            this.Name = name;
            this.Budget = budgetId;
        }

        /// <summary>
        /// Gets the budget this account is associated with.
        /// Off-budget accounts have <see cref="Guid.Empty"/>.
        /// </summary>
        [DataMember(Name = "BudgetId")]
        public BudgetId Budget { get; private set; }

        /// <summary>
        /// Gets the account name
        /// </summary>
        [DataMember(Name = "Name")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the account id
        /// </summary>
        public AccountId AccountId => this.AggregateId;
    }
}
