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

namespace BudgetFirst.Common.Infrastructure.Persistency
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides access to device-specific settings and state (such as device id etc.).
    /// To be implemented per platform.
    /// </summary>
    public interface IDeviceSettings
    {
        /// <summary>
        /// Get the device Id
        /// </summary>
        /// <returns>The current device id</returns>
        Guid GetDeviceId();

        /// <summary>
        /// Set the identifier used to automatically load a budget file on start. Can be <c>null</c>.
        /// </summary>
        /// <param name="identifier">Identifier of the budget</param>
        void SetAutoloadBudgetIdentifier(string identifier);

        /// <summary>
        /// Get the identifier used to automatically load a budget file on start. Can be <c>null</c>.
        /// </summary>
        /// <returns>automatically loaded budget identifier. May be <c>null</c> or empty.</returns>
        string GetAutoloadBudgetIdentifier();

        /// <summary>
        /// Get the list of the recently opened budgets (display name + identifiers)
        /// </summary>
        /// <returns>List of recently opened budgets</returns>
        List<RecentBudget> GetRecentBudgets();

        /// <summary>
        /// Add a new recent budget to the recent budgets
        /// </summary>
        /// <param name="recentBudget">Recent budget</param>
        void AddRecentBudget(RecentBudget recentBudget);
    }
}