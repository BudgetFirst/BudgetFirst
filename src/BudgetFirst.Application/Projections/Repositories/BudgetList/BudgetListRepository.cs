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

namespace BudgetFirst.Application.Projections.Repositories.BudgetList
{
    using BudgetFirst.Application.Projections.Models.BudgetList;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    /// <summary>
    /// Read side budget list repository
    /// </summary>
    public class BudgetListRepository
    {
        /// <summary>
        /// Read store
        /// </summary>
        private IReadStore readStore;

        /// <summary>
        /// Initialises a new instance of the <see cref="BudgetListRepository"/> class.
        /// </summary>
        /// <param name="readStore">Read store</param>
        public BudgetListRepository(IReadStore readStore)
        {
            this.readStore = readStore;
        }

        /// <summary>
        /// Retrieve a budget list from the repository.
        /// </summary>
        /// <returns>Reference to the budget list in the repository. Guaranteed to be not <c>null</c>.</returns>
        public BudgetList Find()
        {
            var list = this.readStore.RetrieveSingleton<BudgetList>();
            if (list == null)
            {
                list = new BudgetList();
                this.Save(list);
            }

            return list;
        }

        /// <summary>
        /// Save the budget list, or add it to the repository. Beware: replaces existing budget list in repository.
        /// </summary>
        /// <param name="budgetList">Budget list to save</param>
        internal void Save(BudgetList budgetList)
        {
            this.readStore.StoreSingleton(budgetList);
        }
    }
}
