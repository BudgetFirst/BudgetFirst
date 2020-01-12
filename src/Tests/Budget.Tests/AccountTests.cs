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

namespace BudgetFirst.Budget.Tests
{
    using System;

    using BudgetFirst.Accounting.Domain.Models;
    using BudgetFirst.Common.Domain.Model.Identifiers;
    using BudgetFirst.Common.Infrastructure.ApplicationState;
    using BudgetFirst.Common.Infrastructure.EventSourcing;
    using BudgetFirst.Common.Infrastructure.Messaging;
    using BudgetFirst.Common.Infrastructure.Persistency;
    using BudgetFirst.SharedInterfaces;

    using NUnit.Framework;

    /// <summary>
    /// Contains tests for the account aggregate
    /// </summary>
    [TestFixture]
    public class AccountTests
    {
        /// <summary>
        /// Unit of work used in tests
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Test setup, runs before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var deviceId = new DeviceId();
            deviceId.SetDeviceId(new Guid("D7BD15B7-AB64-41FE-994D-5DC8E2E8C9D8"));
              
            var vectorClock = new MasterVectorClock(deviceId);
            var eventStore = new EventStore();
            var messageBus = new EventBus();

            this.unitOfWork = new UnitOfWork(deviceId, vectorClock, eventStore, messageBus);
        }

        /// <summary>
        /// A new account must have a correct name
        /// </summary>
        [Test]
        public void NewAccountHasName()
        {
            var offBudgetId = BudgetId.OffBudgetId;
            var account = AccountFactory.Create(new AccountId(new Guid("DB1C3C3E-C8C4-47A0-AD43-F154FDDB0577")), "Test1", offBudgetId, this.unitOfWork);
            Assert.AreEqual("Test1", account.Name);
        }

        /// <summary>
        /// An account, that is loaded from history, must have the correct name
        /// </summary>
        [Test]
        public void ReconstitutedNewAccountHasCorrectName()
        {
            var offBudgetId = BudgetId.OffBudgetId;
            var accountId = new AccountId(new Guid("A34C7724-F9FE-4A14-89A2-C8F1D662EE2A"));
            var prevouslyCreatedAccount = AccountFactory.Create(accountId, "Test2", offBudgetId, this.unitOfWork);
            
            var account = AccountFactory.Load(accountId, this.unitOfWork);

            Assert.AreEqual("Test2", account.Name);
        }
    }
}
