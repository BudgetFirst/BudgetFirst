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
namespace BudgetFirst.Application
{
    using System;

    using BudgetFirst.Application.Messages;
    using BudgetFirst.Application.Projections;
    using BudgetFirst.Common.Infrastructure.Commands;
    using BudgetFirst.Common.Infrastructure.Domain.Events;
    using BudgetFirst.Common.Infrastructure.EventSourcing;
    using BudgetFirst.Common.Infrastructure.Persistency;
    using BudgetFirst.Common.Infrastructure.Projections.Models;

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Represents the Application's core functionality
    /// </summary> 
    public class Kernel 
    {
        /// <summary>
        /// The kernel's bootstrap.
        /// </summary>
        private readonly Bootstrap bootstrap;

        /// <summary>
        /// Access to the device settings
        /// </summary>
        private readonly IDeviceSettings deviceSettings;

        /// <summary>
        /// Access to the application state
        /// </summary>
        private readonly IPersistedApplicationStateRepository persistedApplicationStateRepository;

        /// <summary>
        /// Initialises a new instance of the <see cref="Kernel"/> class.
        /// </summary>
        /// <param name="deviceSettings">Platform-specific device settings</param>
        /// <param name="persistedApplicationStateRepository">Platform-specific repository for the application state</param>
        public Kernel(
            IDeviceSettings deviceSettings, 
            IPersistedApplicationStateRepository persistedApplicationStateRepository)
        {
            this.deviceSettings = deviceSettings;
            this.persistedApplicationStateRepository = persistedApplicationStateRepository;

            var applicationStateFactory = new CurrentApplicationStateFactory(this.GetCurrentApplicationState);

            this.bootstrap = new Bootstrap(persistedApplicationStateRepository, applicationStateFactory);

            var vectorClock = new VectorClock();
            var eventStoreState = new EventStoreState();

            this.bootstrap.EventStore.State = eventStoreState;
            this.bootstrap.VectorClock.SetState(vectorClock);
            this.bootstrap.DeviceId.SetDeviceId(deviceSettings.GetDeviceId());

            this.Repositories = new ReadRepositories(this.bootstrap);
            this.CommandBus = this.bootstrap.CommandBus;

            Messenger.Default.Register<LoadApplicationStateRequested>(this, x => this.LoadApplicationState(x.Location));

            this.ResetReadModelState();
        }

        /// <summary>
        /// Gets the  Application's CommandBus
        /// </summary>
        public ICommandBus CommandBus { get; private set; }

        /// <summary>
        /// Gets the Application's Repositories.
        /// </summary>
        public ReadRepositories Repositories { get; private set; }
        
        /// <summary>
        /// Load application state from disk etc.
        /// This resets all read state, all view models must be rebuilt etc.
        /// </summary>
        /// <param name="location">Application state</param>
        public void LoadApplicationState(string location)
        {
            if (location == null)
            {
                throw new ArgumentNullException();
            }

            var state = this.persistedApplicationStateRepository.Get(location);
            this.bootstrap.VectorClock.SetState(state.VectorClock);
            this.bootstrap.EventStore.State = state.EventStoreState;
            this.ResetReadModelState();

            Messenger.Default.Send(new Messages.LoadedApplicationState());
        }

        /// <summary>
        /// Reset the current read model state and replay all events
        /// </summary>
        internal void ResetReadModelState()
        {
            // TODO: broadcast reset after it is done?
            this.bootstrap.Container.Resolve<IResetableReadStore>().Clear();
            foreach (var @event in this.bootstrap.EventStore.GetEvents())
            {
                this.bootstrap.EventBus.Publish(@event);
            }
        }

        /// <summary>
        /// Get the current application state
        /// </summary>
        /// <returns>Current application state</returns>
        private PersistableApplicationState GetCurrentApplicationState()
        {
            var state = new PersistableApplicationState()
                            {
                                VectorClock =
                                    this.bootstrap.VectorClock.GetVectorClock().Copy(), 
                                EventStoreState = this.bootstrap.EventStore.State.Clone()
                            };

            return state;
        }
    }
}