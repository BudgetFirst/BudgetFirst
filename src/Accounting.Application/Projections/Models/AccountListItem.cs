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

namespace BudgetFirst.Accounting.Application.Projections.Models
{
    using System;

    using BudgetFirst.Accounting.Application.Commands;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.Commands;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    /// <summary>
    /// Account list item, for use in account lists
    /// </summary>
    public class AccountListItem : ReadModel
    {
        /// <summary>
        /// Account name
        /// </summary>
        private string name;

        /// <summary>
        /// Account Id
        /// </summary>
        private AccountId id;

        /// <summary>
        /// Command bus
        /// </summary>
        private ICommandBus commandBus;

        /// <summary>
        /// Initialises a new instance of the <see cref="AccountListItem"/> class.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <param name="name">Account name</param>
        /// <param name="commandBus">Command bus</param>
        public AccountListItem(AccountId id, string name, ICommandBus commandBus)
        {
            this.id = id;
            this.name = name;
            this.commandBus = commandBus;
        }

        /// <summary>
        /// Gets the account Id
        /// </summary>
        public AccountId Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets or sets the account name
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.commandBus.Submit(new ChangeAccountNameCommand() { Id = this.Id, Name = value }); }
        }

        /// <summary>
        /// Set a new account name
        /// </summary>
        /// <param name="name">Account name</param>
        internal void SetName(string name)
        {
            this.SetProperty(ref this.name, name, propertyName: nameof(this.Name));
        }
    }
}
