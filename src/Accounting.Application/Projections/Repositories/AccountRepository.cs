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
    using System;

    using BudgetFirst.Accounting.Application.Projections.Models;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    /// <summary>
    /// Read side account repository
    /// </summary>
    public class AccountRepository 
    {
        /// <summary>
        /// Read store
        /// </summary>
        private IReadStore readStore;

        /// <summary>
        /// Initialises a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="readStore">Read store</param>
        public AccountRepository(IReadStore readStore)
        {
            this.readStore = readStore;
        }

        /// <summary>
        /// Retrieve an account from the repository.
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Reference to the account in the repository, if found. <c>null</c> otherwise.</returns>
        public Account Find(AccountId id)
        {
            return this.readStore.Retrieve<Account>(id.ToGuid());
        }

        /// <summary>
        /// Save the account, or add it to the repository. 
        /// </summary>
        /// <param name="account">Account to save</param>
        public void Save(Account account)
        {
            this.readStore.Store(account.Id.ToGuid(), account);
        }
    }
}
