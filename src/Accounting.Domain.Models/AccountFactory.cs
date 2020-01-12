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

namespace BudgetFirst.Accounting.Domain.Models
{
    using System;

    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Persistency;

    /// <summary>
    /// Factory for <see cref="Account">accounts</see>
    /// </summary>
    public static class AccountFactory
    {
        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="id">Account id</param>
        /// <param name="name">Account name</param>
        /// <param name="budgetId">Budget this account belongs to</param>
        /// <param name="unitOfWork">Current unit of work</param>
        /// <returns>A new account</returns>
        public static Account Create(AccountId id, string name, BudgetId budgetId, IUnitOfWork unitOfWork)
        {
            return new Account(id, name, budgetId, unitOfWork);
        }

        /// <summary>
        /// Load account from (event) history
        /// </summary>
        /// <param name="id">Account id</param>
        /// <param name="unitOfWork">Current unit of work</param>
        /// <returns>An existing account, loaded from the event history</returns>
        public static Account Load(AccountId id, IUnitOfWork unitOfWork)
        {
            return new Account(id, unitOfWork);
        }
    }
}