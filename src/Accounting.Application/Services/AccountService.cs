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

namespace BudgetFirst.Accounting.Application.Services
{
    using BudgetFirst.Accounting.Application.Commands;
    using BudgetFirst.Accounting.Domain.Models;
    using BudgetFirst.Accounting.Infrastructure.Persistence;
    using BudgetFirst.Common.Infrastructure.Commands;
    using BudgetFirst.Common.Infrastructure.Messaging;
    using BudgetFirst.Common.Infrastructure.Persistency;

    /// <summary>
    /// Handles commands related to Accounts
    /// </summary>
    public class AccountService : IHandleCommand<AddAccountCommand>, IHandleCommand<ChangeAccountNameCommand>
    {
        /// <summary>
        /// The Account repository
        /// </summary>
        private readonly AccountRepository repository;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="repository">The account repository</param>
        public AccountService(AccountRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles the ChangeAccountName command
        /// </summary>
        /// <param name="command">The ChangeAccountNameCommand</param>
        /// <param name="unitOfWork">The event transaction</param>
        public void Handle(ChangeAccountNameCommand command, IUnitOfWork unitOfWork)
        {
            var account = this.repository.Find(command.Id, unitOfWork);
            account.ChangeName(command.Name);
        }

        /// <summary>
        /// Handles the CreateAccountName command
        /// </summary>
        /// <param name="command">The CreateAccountNameCommand</param>
        /// <param name="unitOfWork">The event transaction</param>
        public void Handle(AddAccountCommand command, IUnitOfWork unitOfWork)
        {
            var account = AccountFactory.Create(command.Id, command.Name, command.Budget, unitOfWork);
        }
    }
}
