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

namespace BudgetFirst.Application.Projections.Models.BudgetList
{
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Commands;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    /// <summary>
    /// Budget list item, contains details about the budget and related accounts
    /// </summary>
    public class BudgetListItem : ReadModel
    {
        /// <summary>
        /// Account name
        /// </summary>
        private string name;

        /// <summary>
        /// Budget Id
        /// </summary>
        private BudgetId budgetId;

        /// <summary>
        /// Currency code
        /// </summary>
        private string currencyCode;

        /// <summary>
        /// Account list
        /// </summary>
        private AccountList accountList;

        /// <summary>
        /// Command bus
        /// </summary>
        private ICommandBus commandBus;

        /// <summary>
        /// Initialises a new instance of the <see cref="BudgetListItem"/> class.
        /// </summary>
        /// <param name="budgetId">Budget id</param>
        /// <param name="name">Account name</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="accountList">Account list</param>
        /// <param name="commandBus">Command bus</param>
        public BudgetListItem(BudgetId budgetId, string name, string currencyCode, AccountList accountList, ICommandBus commandBus)
        {
            this.budgetId = budgetId;
            this.name = name;
            this.currencyCode = currencyCode;
            this.accountList = accountList;
            this.commandBus = commandBus;
        }

        /// <summary>
        /// Gets the budget Id
        /// </summary>
        public BudgetId BudgetId
        {
            get { return this.budgetId; }
        }

        /// <summary>
        /// Gets or sets the budget name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                // TODO
                // this.commandBus.Submit(new Change);
            }
        }

        /// <summary>
        /// Gets the currency code used in this budget
        /// </summary>
        public string CurrencyCode => this.currencyCode;

        /// <summary>
        /// Gets the account list for this budget
        /// </summary>
        public AccountList Accounts
        {
            get
            {
                return this.accountList;
            }
        }

        /// <summary>
        /// Set a new budget name (from projection)
        /// </summary>
        /// <param name="name">Budget name</param>
        internal void SetName(string name)
        {
            this.SetProperty(ref this.name, name, propertyName: nameof(this.Name));
        }
    }
}
