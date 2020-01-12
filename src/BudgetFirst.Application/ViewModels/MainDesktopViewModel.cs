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
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    using BudgetFirst.Accounting.Application.Commands;
    using BudgetFirst.Application.Messages;
    using BudgetFirst.Application.Projections;
    using BudgetFirst.Application.Projections.Models.BudgetList;
    using BudgetFirst.Application.ViewModels.ReplaceMe;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Persistency;
    using BudgetFirst.Common.Infrastructure.Wrappers;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Represents the main Application for Desktop Platforms.
    /// </summary>
    [ComVisible(false)]
    public class MainDesktopViewModel : ClosableViewModel
    {
        /// <summary>
        /// Current application kernel
        /// </summary>
        private Kernel applicationKernel;

        /// <summary>
        /// The Window Service.
        /// </summary>
        private IWindowService windowService;

        /// <summary>
        /// The Account List.
        /// </summary>
        private AccountList accountList;

        /// <summary>
        /// The account that is selected.
        /// </summary>
        private AccountListItem selectedAccount;

        /// <summary>
        /// Counter to cause rename of account to be different each time
        /// </summary>
        private int renameCount = 0;

        /// <summary>
        /// Initialises a new instance of the <see cref="MainDesktopViewModel"/> class.
        /// </summary>
        /// <param name="windowService">The platform's window service.</param>
        /// <param name="deviceSettings">Platform-specific device settings</param>
        public MainDesktopViewModel(IWindowService windowService, IDeviceSettings deviceSettings)
        {
            this.windowService = windowService;

            Messenger.Default.Register<LoadedApplicationState>(
                this,
                (x) =>
                {
                    this.RebindReadModels();
                });

            this.applicationKernel = ServiceLocatorWrapper.Current.GetInstance<Kernel>();
            var repositories = ServiceLocatorWrapper.Current.GetInstance<ReadRepositories>();

            // this.RebindReadModels(); // this would cause events while this class is not yet initialised
            // so use local fields instead
            // this.accountList = repositories.AccountListRepository.Find(BudgetId.OffBudgetId); // TODO: use correct budget ids etc later
            this.accountList = repositories.BudgetListRepository.Find().Single(x => BudgetId.OffBudgetId.Equals(x.BudgetId)).Accounts;
            this.InitialiseRelayCommands();

            // Only after everything has been initialised, continue with the application flow
            var autoloaded = deviceSettings.GetAutoloadBudgetIdentifier();
            if (!string.IsNullOrWhiteSpace(autoloaded))
            {
                // TODO: error handling, obviously
                this.applicationKernel.LoadApplicationState(autoloaded); // causes message
            }
            else
            {
                // TODO: navigate to "new or load" view
            }
        }

        /// <summary>
        /// Gets the AccountList
        /// </summary>
        public AccountList AccountList
        {
            get
            {
                return this.accountList;
            }

            private set
            {
                this.accountList = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the command to add an Account.
        /// </summary>
        public RelayCommand AddAccountCommand { get; private set; }

        /// <summary>
        /// Gets the command to rename the selected Account.
        /// </summary>
        public RelayCommand RenameAccountCommand { get; private set; }

        /// <summary>
        /// Gets or sets the selected account.
        /// </summary>
        public AccountListItem SelectedAccount
        {
            get
            {
                return this.selectedAccount;
            }

            set
            {
                this.selectedAccount = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        protected override void Close()
        {
            // Run any closing logic then call to the base method to actually close the window.
            // this.windowService.ShowMessage("test");
            base.Close();
        }

        /// <summary>
        /// Initialise all relay command properties
        /// </summary>
        private void InitialiseRelayCommands()
        {
            this.AddAccountCommand = new RelayCommand(this.AddAccount);
            this.RenameAccountCommand = new RelayCommand(this.RenameAccount);
        }

        /// <summary>
        /// Adds a new account.
        /// </summary>
        private void AddAccount()
        {
            // TODO: use actual budget id
            var accountId = new AccountId(Guid.NewGuid());
            var accountName = "Account Name";
            var budgetId = BudgetId.OffBudgetId;
            this.applicationKernel.CommandBus.Submit(new AddAccountCommand(accountId, accountName, budgetId));
        }

        /// <summary>
        /// Renames the selected account.
        /// </summary>
        private void RenameAccount()
        {
            if (this.SelectedAccount == null)
            {
                return;
            }

            // Read models wrap changes directly for us
            this.selectedAccount.Name = $"Renamed Account ({this.renameCount++})";
        }

        /// <summary>
        /// (Re)bind to new read models
        /// </summary>
        private void RebindReadModels()
        {
            // Use properties to cause raise property changed
            this.AccountList = this.applicationKernel.Repositories.BudgetListRepository.Find().Single(x => BudgetId.OffBudgetId.Equals(x.BudgetId)).Accounts;  // TODO: non-hardcoded single budget id
            this.SelectedAccount = null;
        }
    }
}