// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
//
// This source code is licensed under:
//   * GNU General Public License v3.0 (GPLv3)
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

namespace BudgetFirst.Backend.Account
{
    using EventStore;
    using Events;
    using System; 

    public class AccountListItem
    {
        /// <summary>
        /// Account name
        /// </summary>
        private string name;

        /// <summary>
        /// Account Id
        /// </summary>
        private Guid id;

        private EventStore eventStore;

        /// <summary>
        /// Initialises a new instance of the <see cref="AccountListItem"/> class.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <param name="name">Account name</param>
        /// <param name="eventStore">Event store</param>
        public AccountListItem(Guid id, string name, EventStore eventStore)
        {
            this.id = id;
            this.name = name;
            this.eventStore = eventStore;
        }

        /// <summary>
        /// Gets the account Id
        /// </summary>
        public Guid Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets or sets the account name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.eventStore.Add(new AccountCreated(this.Id, this.Name));
            }
        }
    }
}
