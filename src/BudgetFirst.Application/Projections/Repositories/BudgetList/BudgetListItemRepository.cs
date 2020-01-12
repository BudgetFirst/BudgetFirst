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
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    /// <summary>
    /// Read side budget list item repository
    /// </summary>
    public class BudgetListItemRepository 
    {
        /// <summary>
        /// Read store
        /// </summary>
        private IReadStore readStore;

        /// <summary>
        /// Initialises a new instance of the <see cref="BudgetListItemRepository"/> class.
        /// </summary>
        /// <param name="readStore">Read store</param>
        public BudgetListItemRepository(IReadStore readStore)
        {
            this.readStore = readStore;
        }

        /// <summary>
        /// Retrieve a budget list item from the repository.
        /// </summary>
        /// <param name="id">Budget Id</param>
        /// <returns>Reference to the budget list item in the repository, if found. <c>null</c> otherwise.</returns>
        public BudgetListItem Find(BudgetId id)
        {
            return this.readStore.Retrieve<BudgetListItem>(id.ToGuid());
        }

        /// <summary>
        /// Save the budget list item, or add it to the repository. 
        /// </summary>
        /// <param name="budget">Budget list item to save</param>
        internal void Save(BudgetListItem budget)
        {
            this.readStore.Store(budget.BudgetId.ToGuid(), budget);
        }
    }
}
