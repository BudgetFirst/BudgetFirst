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

namespace BudgetFirst.Application.ViewModels.ReplaceMe
{
    /// <summary>
    /// Interface for working with windows in a non-platform specific way.
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Shows a Window for a certain ViewModel
        /// </summary>
        /// <typeparam name="T">The type of the ViewModel, it must subclass <see cref="ClosableViewModel"/></typeparam>
        /// <param name="viewModel">The ViewModel to open a window for.</param>
        /// <returns>The Window that was created.</returns>
        object ShowWindow<T>(T viewModel) where T : ClosableViewModel;

        /// <summary>
        /// Show's a message to the user.
        /// </summary>
        /// <param name="message">The message to show.</param>
        void ShowMessage(string message);
    }
}
