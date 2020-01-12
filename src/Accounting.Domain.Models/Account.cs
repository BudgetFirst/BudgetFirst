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
    using System.Runtime.InteropServices;

    using BudgetFirst.Accounting.Domain.Events;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Domain.Model;
    using BudgetFirst.Common.Infrastructure.Persistency;

    /// <summary>
    /// An account, as in bank account, wallet etc.
    /// </summary>
    [ComVisible(false)]
    public class Account : AggregateRoot<AccountId>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Account"/> class.
        /// Create a new account
        /// </summary>
        /// <param name="id">Account id</param>
        /// <param name="name">Account name</param>
        /// <param name="budgetId">Budget the account belongs to</param>
        /// <param name="unitOfWork">Unit of work</param>
        internal Account(AccountId id, string name, BudgetId budgetId, IUnitOfWork unitOfWork) : this(id, unitOfWork, false)
        {
            if (id == null || !id.IsValid())
            {
                throw new ArgumentException("Account id must be valid");
            }

            // TODO: value object instead
            if (string.IsNullOrWhiteSpace(name)) 
            {
                throw new ArgumentException("Account name must be valid");
            }

            if (budgetId == null)
            {
                throw new ArgumentNullException();
            }

            this.Apply(new AddedAccount(name, budgetId));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Account"/> class.
        /// Load account from history
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <param name="unitOfWork">Unit of work</param>
        internal Account(AccountId id, IUnitOfWork unitOfWork) : this(id, unitOfWork, true)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Account"/> class.
        /// Serves as a base for all <see cref="Account"/> constructors.
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="loadFromHistory">Load the aggregate state from history</param>
        /// <remarks>Load from history cannot be part of the base class because we must define the event handlers first</remarks>
        private Account(AccountId id, IUnitOfWork unitOfWork, bool loadFromHistory) : base(id, unitOfWork)
        {
            this.Handles<AddedAccount>(this.When);
            this.Handles<AccountNameChanged>(this.When);

            if (loadFromHistory)
            {
                this.LoadFrom(unitOfWork.GetEventsForAggregate(id));
            }
        }

        /// <summary>
        /// Gets the account name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Changes the name of the account and emits a AccountNameChanged event
        /// </summary>
        /// <param name="newName">The new name of the account</param>
        public void ChangeName(string newName)
        {
            this.Apply(new AccountNameChanged(newName));
        }

        /// <summary>
        /// Handles <see cref="AddedAccount"/> events
        /// </summary>
        /// <param name="e">Event to handle</param>
        private void When(AddedAccount e)
        {
            this.Name = e.Name;
        }

        /// <summary>
        /// Handles <see cref="AccountNameChanged"/> events
        /// </summary>
        /// <param name="e">Event to handle</param>
        private void When(AccountNameChanged e)
        {
            this.Name = e.Name;
        }
    }
}
