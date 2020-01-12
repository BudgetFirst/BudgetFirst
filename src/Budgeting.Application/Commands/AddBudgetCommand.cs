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

namespace BudgetFirst.Budgeting.Application.Commands
{
    using System;

    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Commands;

    /// <summary>
    /// Command to create a new budget
    /// </summary>
    public class AddBudgetCommand : ICommand
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AddBudgetCommand" /> class.
        /// </summary>
        /// <param name="id">Unique Id of the budget</param>
        /// <param name="name">Name of the budget</param>
        /// <param name="currencyCode">Currency code (e.g. EUR)</param>
        public AddBudgetCommand(BudgetId id, string name, string currencyCode)
        {
            this.Id = id;
            this.Name = name;
            this.CurrencyCode = currencyCode;
        }

        /// <summary>
        /// Gets or sets the budget Id
        /// </summary>
        public BudgetId Id { get; set; }

        /// <summary>
        /// Gets or sets the budget name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the currency code
        /// </summary>
        /// <remarks>TODO: currency code should be a separate type (value object; currency context is shared kernel)</remarks>
        public string CurrencyCode { get; set; }
    }
}