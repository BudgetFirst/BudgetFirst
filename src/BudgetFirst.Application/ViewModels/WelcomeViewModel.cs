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

namespace BudgetFirst.Application.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;

    using BudgetFirst.Application.Commands.Infrastructure;
    using BudgetFirst.Application.Messages;
    using BudgetFirst.Common.Infrastructure.Commands;
    using BudgetFirst.Common.Infrastructure.Persistency;
    using BudgetFirst.Common.Infrastructure.Wrappers;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;

    /// <summary>
    /// Initial page view model. Contains steps to open or create a budget.
    /// </summary>
    [ComVisible(false)]
    public class WelcomeViewModel : ViewModelBase
    {
        /// <summary>
        /// Backing state for notify-able property
        /// </summary>
        private ObservableCollection<RecentBudget> recentBudgets;

        /// <summary>
        /// Initialises a new instance of the <see cref="WelcomeViewModel"/> class.
        /// </summary>
        /// <param name="deviceSettings">Device settings</param>
        public WelcomeViewModel(IDeviceSettings deviceSettings)
        {
            // Initialise list of recent budgets
            this.RebindViewModel(deviceSettings);

            // Initialise relay commands
            this.CreateNewBudgetCommand = new RelayCommand(
                () =>
                    {
                        var navigationService = ServiceLocatorWrapper.Current.GetInstance<INavigationService>();
                        navigationService.NavigateTo(ViewModelPageKeys.CreateNewBudget);
                    });

            this.OpenExistingBudgetCommand = new RelayCommand(
                () =>
                    {
                        var navigationService = ServiceLocatorWrapper.Current.GetInstance<INavigationService>();
                        navigationService.NavigateTo(ViewModelPageKeys.OpenExistingBudget);
                    });

            // Messaging
            Messenger.Default.Register<LoadedApplicationState>(
                this,
                (x) =>
                    {
                        // TODO: always trigger navigation to primary after opening budget?
                        // trigger navigation (if we're still the displayed view)
                        var navigationService = ServiceLocatorWrapper.Current.GetInstance<INavigationService>();
                        if (navigationService.CurrentPageKey == ViewModelPageKeys.Welcome)
                        {
                            navigationService.NavigateTo(ViewModelPageKeys.PrimaryApplication);
                        }

                        // Also, rebind.
                        this.RebindViewModel(deviceSettings);
                        this.RaisePropertyChangedForReboundProperties();
                    });
        }

        /// <summary>
        /// Gets the create new budget command
        /// </summary>
        /// <remarks>This property never changes</remarks>
        public RelayCommand CreateNewBudgetCommand { get; }
        
        /// <summary>
        /// Gets the open existing budget command (triggers navigation to open existing budget)
        /// </summary>
        /// <remarks>This property never changes</remarks>
        public RelayCommand OpenExistingBudgetCommand { get; }

        /// <summary>
        /// Gets the recent budgets
        /// </summary>
        public ObservableCollection<RecentBudget> RecentBudgets
        {
            get
            {
                return this.recentBudgets;
            }

            private set
            {
                this.recentBudgets = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// (Re)bind the view model to the new application state
        /// </summary>
        /// <param name="deviceSettings">Device settings</param>
        private void RebindViewModel(IDeviceSettings deviceSettings)
        {
            var recentlyUsedBudgets = deviceSettings.GetRecentBudgets() ?? new List<Common.Infrastructure.Persistency.RecentBudget>();

            // Map to viewmodel-specific model
            var viewModelRecentBudgets =
                recentlyUsedBudgets.Select(
                    x => new RecentBudget()
                    {
                        Name = x.DisplayName,
                        OpenCommand = new RelayCommand(
                                     () =>
                                         {
                                             var commandBus = ServiceLocatorWrapper.Current.GetInstance<ICommandBus>();
                                             commandBus.Submit(new LoadApplicationState(x.Identifier));

                                             // Navigation is done when the corresponding message reaches us (and we're still responsible for the navigation)
                                         }),
                    });

            // Do not use the property right now
            this.recentBudgets = new ObservableCollection<RecentBudget>(viewModelRecentBudgets);
        }

        /// <summary>
        /// Raise property changed for properties that have been set in <see cref="RebindViewModel"/>.
        /// This is a separate method to avoid raising notify property changed during the constructor
        /// </summary>
        private void RaisePropertyChangedForReboundProperties()
        {
            this.RecentBudgets = this.recentBudgets;
        }

        /// <summary>
        /// View model specific recent budget representation
        /// </summary>
        public class RecentBudget
        {
            /// <summary>
            /// Gets or sets the name of the recent budget
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the command to invoke when opening the budget
            /// </summary>
            public RelayCommand OpenCommand { get; set; }
        }
    }
}