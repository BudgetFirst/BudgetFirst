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
    using System;
    using System.Runtime.InteropServices;

    using BudgetFirst.Application.Projections;
    using BudgetFirst.Budgeting.Application.Commands;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Wrappers;
    using BudgetFirst.Currencies.Application.Projections.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// View model for creating a new budget
    /// </summary>
    [ComVisible(false)]
    public class CreateNewBudgetViewModel : ViewModelBase
    {
        /// <summary>
        /// Name of the budget
        /// </summary>
        private string name;

        /// <summary>
        /// List of currencies
        /// </summary>
        private CurrencyList currencyList;

        /// <summary>
        /// Selected currency
        /// </summary>
        private Currency selectedCurrency;

        /// <summary>
        /// Initialises a new instance of the <see cref="CreateNewBudgetViewModel"/> class.
        /// </summary>
        /// <param name="readRepositories">Repositories for read models</param>
        public CreateNewBudgetViewModel(ReadRepositories readRepositories)
        {
            this.currencyList = readRepositories.CurrencyRepository.GetAll();

            this.CreateNewBudget = new RelayCommand(
                () =>
                {
                    var budgetId = new BudgetId(Guid.NewGuid());
                    var currencyCode = this.SelectedCurrency?.Code;
                    ServiceLocatorWrapper.Current.GetInstance<Kernel>().CommandBus.Submit(new AddBudgetCommand(budgetId, this.Name, currencyCode));
                    
                    // TODO: navigate?
                },
                () => !string.IsNullOrWhiteSpace(this.Name) && this.selectedCurrency != null);
        }

        /// <summary>
        /// Gets the create new budget command
        /// </summary>
        public RelayCommand CreateNewBudget { get; private set; }

        /// <summary>
        /// Gets or sets the budget name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the list of currencies
        /// </summary>
        public CurrencyList CurrencyList
        {
            get
            {
                return this.currencyList;
            }

            private set
            {
                this.currencyList = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected currency
        /// </summary>
        public Currency SelectedCurrency
        {
            get
            {
                return this.selectedCurrency;
            }

            set
            {
                this.selectedCurrency = value;
                this.RaisePropertyChanged();
            }
        }
    }
}