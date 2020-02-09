﻿// BudgetFirst 
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


using System.Collections.ObjectModel;

namespace BudgetFirst.Backend.Account
{
    using EventStore;
    using Events;
    using System;
    using System.Linq;

    public class AccountList : ObservableCollection<AccountListItem>
    {
        private EventStore eventStore;

        public AccountList(EventStore eventStore)
        {
            if(eventStore is null)
            {
                throw new ArgumentNullException(nameof(eventStore));
            }
            this.eventStore = eventStore;
            this.eventStore.SubscribeTo<AccountCreated>(this.Handle);
        }

        private void Handle(AccountCreated accountCreated) {
            this.Add(new AccountListItem(accountCreated.Id, accountCreated.Name, eventStore));
        }

        private void Handle(AccountRenamed accountRenamed)
        {
            var account = this.Items.Single(x => x.Id == accountRenamed.Id);
            account.Name = accountRenamed.NewName;
        }
    }
}
