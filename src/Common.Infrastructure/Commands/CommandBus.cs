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

namespace BudgetFirst.Common.Infrastructure.Commands
{
    using BudgetFirst.Common.Infrastructure.ApplicationState;
    using BudgetFirst.Common.Infrastructure.DependencyInjection;
    using BudgetFirst.Common.Infrastructure.EventSourcing;
    using BudgetFirst.Common.Infrastructure.Messaging;
    using BudgetFirst.Common.Infrastructure.Persistency;

    /// <summary>
    /// The command bus accepts commands and forwards them to the corresponding command handler.
    /// </summary>
    public class CommandBus : ICommandBus
    {
        /// <summary>
        /// Dependency injection container, needed to resolve command handler
        /// </summary>
        private IContainer dependencyInjectionContainer;

        /// <summary>
        /// Event bus, needed to publish events to
        /// </summary>
        /// <remarks>TODO: event publishing should be job of event store?</remarks>
        private IEventBus eventBus;

        /// <summary>
        /// Current device Id
        /// </summary>
        private IReadOnlyDeviceId readOnlyDeviceId;

        /// <summary>
        /// Current vector clock
        /// </summary>
        private IVectorClock vectorClock;

        /// <summary>
        /// Current event store
        /// </summary>
        private IEventStore eventStore;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandBus"/> class. 
        /// </summary>
        /// <param name="dependencyInjector">Dependency injection/resolving container</param>
        /// <param name="eventBus">Message bus to publish events to</param>
        /// <param name="readOnlyDeviceId">Device id</param>
        /// <param name="vectorClock">current vector clock</param>
        /// <param name="eventStore">Event store</param>
        public CommandBus(
            IContainer dependencyInjector,
            IEventBus eventBus,
            IReadOnlyDeviceId readOnlyDeviceId,
            IVectorClock vectorClock,
            IEventStore eventStore)
        {
            this.dependencyInjectionContainer = dependencyInjector;
            this.eventBus = eventBus;
            this.readOnlyDeviceId = readOnlyDeviceId;
            this.vectorClock = vectorClock;
            this.eventStore = eventStore;
        }

        /// <summary>
        /// Submit a command to the corresponding handler
        /// </summary>
        /// <typeparam name="TCommand">Command type</typeparam>
        /// <param name="command">Command to be executed</param>
        public void Submit<TCommand>(TCommand command) where TCommand : ICommand
        {
            var unitOfWork = new UnitOfWork(this.readOnlyDeviceId, this.vectorClock, this.eventStore, this.eventBus);
            this.InvokeHandler(command, unitOfWork);
            unitOfWork.Commit(); // handles saving, publishing, vector clock etc.
        }

        /// <summary>
        /// Resolve and invoke the corresponding command handler
        /// </summary>
        /// <typeparam name="TCommand">Command type</typeparam>
        /// <param name="command">Command to execute</param>
        /// <param name="unitOfWork">Event transaction to track unpublished events</param>
        private void InvokeHandler<TCommand>(TCommand command, IUnitOfWork unitOfWork) where TCommand : ICommand
        {
            var handler = this.dependencyInjectionContainer.Resolve<IHandleCommand<TCommand>>();
            handler.Handle(command, unitOfWork);
        }
    }
}
