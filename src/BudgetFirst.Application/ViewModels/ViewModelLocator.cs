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

namespace BudgetFirst.Application.ViewModels
{
    using BudgetFirst.Application.Projections;
    using BudgetFirst.Common.Infrastructure.Persistency;
    using BudgetFirst.Common.Infrastructure.Wrappers;

    /// <summary>
    /// View model locator
    /// </summary>
    /// <typeparam name="TDeviceSettings">Platform-specific implementing class of <see cref="IDeviceSettings"/></typeparam>
    /// <typeparam name="TPersistedApplicationStateRepository">Platform-specific implementing class of <see cref="IPersistedApplicationStateRepository"/></typeparam>
    public class ViewModelLocator<TDeviceSettings, TPersistedApplicationStateRepository>
        where TDeviceSettings : class, IDeviceSettings, new()
        where TPersistedApplicationStateRepository : class, IPersistedApplicationStateRepository, new()
    {
        /// <summary>
        /// Initialises static members of the <see cref="ViewModelLocator{TDeviceSettings,TPersistedApplicationStateRepository}"/> class.
        /// </summary>
        /// <remarks>This is static because we had issues with multiple instances being created in visual studio by the designer in wpf projects</remarks>
        static ViewModelLocator()
        {
            // Every object in SimpleIoc is singleton by default
            SimpleIocWrapper.Default.Register<IDeviceSettings, TDeviceSettings>();
            SimpleIocWrapper.Default.Register<IPersistedApplicationStateRepository, TPersistedApplicationStateRepository>();

            // View models
            SimpleIocWrapper.Default.Register<MainDesktopViewModel>();
            SimpleIocWrapper.Default.Register<WelcomeViewModel>();
            SimpleIocWrapper.Default.Register<CreateNewBudgetViewModel>();

            // Application repositories and kernel
            SimpleIocWrapper.Default.Register<Kernel>();
            SimpleIocWrapper.Default.Register<ReadRepositories>(() => SimpleIocWrapper.Default.GetInstance<Kernel>().Repositories);
        }

        /// <summary>
        /// Gets the welcome view model
        /// </summary>
        public WelcomeViewModel WelcomeViewModel => ServiceLocatorWrapper.Current.GetInstance<WelcomeViewModel>();

        /// <summary>
        /// Ensure that the static class has been initialised
        /// </summary>
        public static void EnsureInitialised()
        {
        }
    }
}