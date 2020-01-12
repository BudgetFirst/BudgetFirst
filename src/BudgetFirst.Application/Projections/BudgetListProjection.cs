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
namespace BudgetFirst.Application.Projections
{
    using System;
    using System.Linq;

    using BudgetFirst.Accounting.Domain.Events;
    using BudgetFirst.Application.Projections.Models.BudgetList;
    using BudgetFirst.Application.Projections.Repositories.BudgetList;
    using BudgetFirst.Budgeting.Domain.Events;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Commands;
    using BudgetFirst.Common.Infrastructure.Projections;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    /// <summary>
    /// Projection for budget lists
    /// </summary>
    public class BudgetListProjection : IProjectFrom<AddedBudget>, 
                                        IProjectFrom<AddedAccount>, 
                                        IProjectFrom<AccountNameChanged> // any new handler must be registered in bootstrap
    {
        /// <summary>
        /// Account list item repository
        /// </summary>
        private AccountListItemRepository accountListItemRepository;

        /// <summary>
        /// Account list repository
        /// </summary>
        private AccountListRepository accountListRepository;

        /// <summary>
        /// Budget list item repository
        /// </summary>
        private BudgetListItemRepository budgetListItemRepository;

        /// <summary>
        /// Budget list repository
        /// </summary>
        private BudgetListRepository budgetListRepository;

        /// <summary>
        /// Command bus
        /// </summary>
        private ICommandBus commandBus;

        /// <summary>
        /// Initialises a new instance of the <see cref="BudgetListProjection"/> class.
        /// </summary>
        /// <param name="budgetListRepository">Budget list repository to use</param>
        /// <param name="budgetListItemRepository">Budget list item repository to use</param>
        /// <param name="accountListRepository">Account list repository to use</param>
        /// <param name="accountListItemRepository">Account list item repository to use</param>
        /// <param name="commandBus">Command bus</param>
        /// <param name="readStoreResetBroadcast">Read store reset broadcast</param>
        public BudgetListProjection(
            BudgetListRepository budgetListRepository, 
            BudgetListItemRepository budgetListItemRepository, 
            AccountListRepository accountListRepository, 
            AccountListItemRepository accountListItemRepository, 
            ICommandBus commandBus, 
            IReadStoreResetBroadcast readStoreResetBroadcast)
        {
            this.budgetListRepository = budgetListRepository;
            this.budgetListItemRepository = budgetListItemRepository;
            this.accountListRepository = accountListRepository;
            this.accountListItemRepository = accountListItemRepository;
            this.commandBus = commandBus;

            readStoreResetBroadcast.ReadStoreReset +=
                (sender, args) =>
                    {
                        this.Init(budgetListRepository, budgetListItemRepository, accountListRepository, commandBus);
                    };

            this.Init(budgetListRepository, budgetListItemRepository, accountListRepository, commandBus);
        }

        /// <summary>
        /// Budget created/added event handler
        /// </summary>
        /// <param name="e">Budget created event</param>
        public void Handle(AddedBudget e)
        {
            var budgetList = this.GetBudgetList();

            var budget = budgetList.FirstOrDefault(x => e.BudgetId.Equals(x.BudgetId));
            if (budget == null)
            {
                var accountList = this.GetAccountListForBudget(e.BudgetId);
                budget = new BudgetListItem(e.BudgetId, e.Name, e.CurrencyCode, accountList, this.commandBus);
                this.budgetListItemRepository.Save(budget);
                budgetList.Add(budget);
            }

            budget.SetName(e.Name);

            this.budgetListRepository.Save(budgetList);
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
                // TODO: handle gracefully?
                throw new InvalidOperationException(
                    "Account list item with id " + e.AccountId.ToString() + " was not found in repository.");
            }

            account.SetName(e.Name);
            this.accountListItemRepository.Save(account);
        }

        /// <summary>
        /// Get the initialised account list for a budget
        /// </summary>
        /// <param name="budgetId">Budget id</param>
        /// <returns>Initialised account list</returns>
        private AccountList GetAccountListForBudget(BudgetId budgetId)
        {
            var accountList = this.GetBudgetList().FirstOrDefault(x => budgetId.Equals(x.BudgetId)).Accounts;
            if (accountList == null)
            {
                accountList = new AccountList();
                this.accountListRepository.Save(budgetId, accountList);
            }

            return accountList;
        }

        /// <summary>
        /// Get an initialized budget list
        /// </summary>
        /// <returns>initialized budget list</returns>
        private BudgetList GetBudgetList()
        {
            var budgetList = this.budgetListRepository.Find();
            if (budgetList == null)
            {
                budgetList = new BudgetList();
                this.budgetListRepository.Save(budgetList);
            }

            return budgetList;
        }

        /// <summary>
        /// (Re)initialization to provide initial values not derived from events
        /// </summary>
        /// <param name="budgetListRepository">budget list repository</param>
        /// <param name="budgetListItemRepository">budget list item repository</param>
        /// <param name="accountListRepository">account list repository</param>
        /// <param name="commandBus">Command bus</param>
        private void Init(
            BudgetListRepository budgetListRepository, 
            BudgetListItemRepository budgetListItemRepository, 
            AccountListRepository accountListRepository, 
            ICommandBus commandBus)
        {
            // Init with "off-budget" budget
            var list = this.GetBudgetList();
            var offBudgetAccountList = new AccountList();
            var offBudget = new BudgetListItem(
                BudgetId.OffBudgetId, 
                "Off-budget", 
                null, 
                offBudgetAccountList, 
                commandBus);
            list.Add(offBudget);
            accountListRepository.Save(offBudget.BudgetId, offBudgetAccountList);
            budgetListItemRepository.Save(offBudget);
            budgetListRepository.Save(list);
        }
    }
}