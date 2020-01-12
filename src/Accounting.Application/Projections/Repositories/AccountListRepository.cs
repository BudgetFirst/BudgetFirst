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

namespace BudgetFirst.Accounting.Application.Projections.Repositories
{
    using BudgetFirst.Accounting.Application.Projections.Models;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    /// <summary>
    /// Read side account list repository
    /// </summary>
    public class AccountListRepository 
    {
        /// <summary>
        /// Read store
        /// </summary>
        private IReadStore readStore;

        /// <summary>
        /// Initialises a new instance of the <see cref="AccountListRepository"/> class.
        /// </summary>
        /// <param name="readStore">Read store</param>
        public AccountListRepository(IReadStore readStore)
        {
            this.readStore = readStore;
        }

        /// <summary>
        /// Retrieve an account list from the repository.
        /// </summary>
        /// <param name="budget">Budget the accounts belong to</param>
        /// <returns>Reference to the account list in the repository. Guaranteed to be not <c>null</c>.</returns>
        public AccountList Find(BudgetId budget)
        {
            var list = this.readStore.Retrieve<AccountList>(budget.ToGuid());
            if (list == null)
            {
                list = new AccountList();
                this.Save(budget, list);
            }

            return list;
        }

        /// <summary>
        /// Save the account list, or add it to the repository. Beware: replaces existing account list in repository.
        /// </summary>
        /// <param name="budget">Budget the accounts belong to</param>
        /// <param name="accountList">Account list to save</param>
        internal void Save(BudgetId budget, AccountList accountList)
        {
            this.readStore.Store(budget.ToGuid(), accountList);
        }
    }
}
