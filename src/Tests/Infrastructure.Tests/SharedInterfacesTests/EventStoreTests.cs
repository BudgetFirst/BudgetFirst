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

namespace BudgetFirst.Infrastructure.Tests.SharedInterfacesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    using BudgetFirst.Common.Infrastructure.Domain.Events;
    using BudgetFirst.Common.Infrastructure.Domain.Model;
    using BudgetFirst.Common.Infrastructure.EventSourcing;
    using BudgetFirst.Common.Infrastructure.Messaging;
    using BudgetFirst.Common.Infrastructure.Serialisation;

    using NUnit.Framework;

    /// <summary>
    /// Contains tests for the event store
    /// </summary>
    [TestFixture]
    public class EventStoreTests
    {
        /// <summary>
        /// Added events are in the store
        /// </summary>
        [Test]
        public void AddedEventsAreInStore()
        {
            var eventStore = new EventStore();
            var @event = new TestEvent();
            @event.AggregateId = new TestEventId(new Guid("FFF291EF-1911-4C7C-BB30-89C29F0D6D3E"));
            @event.DeviceId = new Guid("6BAAB117-6D9B-404A-AB6A-4AE7A8DA856C");
            @event.VectorClock = new VectorClock();

            eventStore.Add(@event);

            Assert.Contains(@event, eventStore.GetEvents().ToList());
        }

        /// <summary>
        /// Only the added events are in the store, no other events
        /// </summary>
        [Test]
        public void OnlyAddedEventsAreInStore()
        {
            var eventStore = new EventStore();
            var @event = new TestEvent();
            @event.AggregateId = new TestEventId(new Guid("FFF291EF-1911-4C7C-BB30-89C29F0D6D3E"));
            @event.DeviceId = new Guid("6BAAB117-6D9B-404A-AB6A-4AE7A8DA856C");
            @event.VectorClock = new VectorClock();

            eventStore.Add(@event);

            var events = eventStore.GetEvents().ToList();
            Assert.That(() => events.All(x => ReferenceEquals(x, @event) && events.Count == 1));
        }

        /// <summary>
        /// Adding an invalid (incomplete) event throws an exception
        /// </summary>
        [Test]
        public void AddingInvalidEventThrowsException()
        {
            var eventStore = new EventStore();
            var @event = new TestEvent();
            /* Do not set aggregate id etc. */

            Assert.Throws<DomainEventIncompleteException>(() => eventStore.Add(@event));
        }

        /// <summary>
        /// A new event store is empty
        /// </summary>
        [Test]
        public void NewEventStoreIsEmpty()
        {
            var eventStore = new EventStore();

            Assert.IsEmpty(eventStore.GetEvents());
        }

        /// <summary>
        /// Events survive a serialisation round trip
        /// </summary>
        [Test]
        public void EventsSurviveSerialisationRoundtrip()
        {
            var eventStore = new EventStore();
            var @event = new TestEvent();
            @event.AggregateId = new TestEventId(new Guid("FFF291EF-1911-4C7C-BB30-89C29F0D6D3E"));
            @event.DeviceId = new Guid("6BAAB117-6D9B-404A-AB6A-4AE7A8DA856C");
            @event.VectorClock = new VectorClock();

            eventStore.Add(@event);

            Serialiser.KnownTypes = new Type[]
                                        {
                                            typeof(TestEvent),
                                            typeof(TestEventId),
                                        };

            var clonedState = Serialiser.CloneSerialisable(eventStore.State);

            Assert.That(() => eventStore.State.Events.SequenceEqual(clonedState.Events));
        }

        /// <summary>
        /// Simple, empty event for tests
        /// </summary>
        [DataContract]
        private class TestEvent : DomainEvent<TestEventId>
        {
        }

        /// <summary>
        /// Simple test id
        /// </summary>
        [DataContract]
        private sealed class TestEventId : AggregateId
        {
            /// <summary>
            /// Initialises a new instance of the <see cref="TestEventId"/> class.
            /// </summary>
            /// <param name="id">Underlying id</param>
            public TestEventId(Guid id) : base(id)
            {
            }
        }
    }
}
