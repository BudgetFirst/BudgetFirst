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

namespace BudgetFirst.Accounting.Application.Projections
{
    using System;
    using System.Linq;

    using BudgetFirst.Accounting.Application.Projections.Models;
    using BudgetFirst.Accounting.Application.Projections.Repositories;
    using BudgetFirst.Accounting.Domain.Events;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Commands;
    using BudgetFirst.Common.Infrastructure.Projections;

    /// <summary>
    /// Projection for account lists
    /// </summary>
    public class AccountListProjection : IProjectFrom<AddedAccount>, IProjectFrom<AccountNameChanged> // any new handler must be registered in bootstrap
    {
        /// <summary>
        /// Account list repository
        /// </summary>
        private AccountListRepository accountListRepository;

        /// <summary>
        /// Account list item repository
        /// </summary>
        private AccountListItemRepository accountListItemRepository;

        /// <summary>
        /// Command bus
        /// </summary>
        private ICommandBus commandBus;

        /// <summary>
        /// Initialises a new instance of the <see cref="AccountListProjection"/> class.
        /// </summary>
        /// <param name="accountListRepository">Account list repository to use</param>
        /// <param name="accountListItemRepository">Account list item repository to use</param>
        /// <param name="commandBus">Command bus</param>
        public AccountListProjection(
            AccountListRepository accountListRepository, 
            AccountListItemRepository accountListItemRepository,
            ICommandBus commandBus)
        {
            this.accountListRepository = accountListRepository;
            this.accountListItemRepository = accountListItemRepository;
            this.commandBus = commandBus;
        }

        /// <summary>
        /// Account created event handler
        /// </summary>
        /// <param name="e">Account created event</param>
        public void Handle(AddedAccount e)
        {
            var accountList = this.GetAccountListForBudget(e.Budget);
            
            var account = accountList.FirstOrDefault(x => e.AccountId.Equals(x.Id));
            if (account == null)
            {
                account = new AccountListItem(e.AccountId, e.Name, this.commandBus);
                this.accountListItemRepository.Save(account);
                accountList.Add(account);
            }

            account.SetName(e.Name);

            this.accountListRepository.Save(e.Budget, accountList);
        }

        /// <summary>
        /// Account name changed event handler
        /// </summary>
        /// <param name="e">Account renamed event</param>
        public void Handle(AccountNameChanged e)
        {
            // Only update account list item
            var account = this.accountListItemRepository.Find(e.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException("Account list item with id " + e.AccountId.ToString() + " was not found in repository.");
            }

            account.SetName(e.Name);
            this.accountListItemRepository.Save(account);
        }

        /// <summary>
        /// Get the initialised account list
        /// </summary>
        /// <param name="budgetId">Budget id</param>
        /// <returns>Initialised account list</returns>
        private AccountList GetAccountListForBudget(BudgetId budgetId)
        {
            var accountList = this.accountListRepository.Find(budgetId);
            if (accountList == null)
            {
                accountList = new AccountList();
                this.accountListRepository.Save(budgetId, accountList);
            }

            return accountList;
        }
    }
}
