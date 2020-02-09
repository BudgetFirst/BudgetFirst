// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
//
// This source code is licensed under:
//   * GNU General Public License v3.0 (GPLv3)
//
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

namespace BudgetFirst.Backend.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A simple event store
    /// </summary>
    /// <remarks>Not thread-safe, does not yet include any persistence (which should be handled outside this class anyway)</remarks>
    public class EventStore
    {
        /// <summary>
        /// All saved events
        /// </summary>
        private EventStoreState state; // = new EventStoreState();
        private DeviceId deviceId;
        private DeviceVectorClock deviceVectorClock;

        private Dictionary<Type, List<Action<Event>>> subscribers;

        public EventStore(EventStoreState state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }
            this.state = state;
            this.subscribers = new Dictionary<Type, List<Action<Event>>>();
            this.deviceId = new DeviceId() { Id = Guid.NewGuid() };
            this.deviceVectorClock = new DeviceVectorClock(this.deviceId);
        }

        public EventStore(EventStoreState state, DeviceId deviceId)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }
            if (deviceId is null)
            {
                throw new ArgumentNullException(nameof(deviceId));
            }
            this.state = state;
            this.subscribers = new Dictionary<Type, List<Action<Event>>>();
            this.deviceId = new DeviceId() { Id = deviceId.Id };
            this.deviceVectorClock = new DeviceVectorClock(this.deviceId);
        }

        /// <summary>
        /// Gets or sets state (i.e. all events)
        /// </summary>
        public EventStoreState State
        {
            get
            {
                return this.state;
            }

            set
            {
                this.state = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Get all saved events.
        /// Beware: events are referenced directly, do not manipulate them.
        /// </summary>
        /// <returns>References to all saved events</returns>
        public IReadOnlyList<Event> GetEvents()
        {
            return this.state.Events;
        }

        ///// <summary>
        ///// Save multiple events
        ///// </summary>
        ///// <param name="newEvents">Events to save</param>
        //public void Add<TEvent>(IEnumerable<Event> newEvents)
        //{
        //    var newEventList = newEvents.ToList();
        //    foreach (var @event in newEventList)
        //    {
        //        this.CheckValidity(@event);
        //        this.ApplyVectorClock(@event);
        //    }

        //    this.state.Events.AddRange(newEventList);
        //    this.NotifySubscribers(newEventList);
        //}

        /// <summary>
        /// Save a single event
        /// </summary>
        /// <param name="newEvent">Event to save</param>
        public void Add<TEvent>(TEvent newEvent) where TEvent : Event
        {
            if (newEvent is null)
            {
                throw new ArgumentNullException(nameof(newEvent));
            }
            this.CheckValidity(newEvent);
            this.ApplyVectorClock(newEvent);
            this.state.Events.Add(newEvent);
            this.NotifySubscribers(newEvent);
        }

        // TODO: bad delegate, makes it impossible to remove later. is this a problem? also not so nice
        public void SubscribeTo<TEvent>(Action<TEvent> action) where TEvent : Event
        {
            var eventType = typeof(TEvent);
            if (!this.subscribers.TryGetValue(eventType, out var actions))
            {
                actions = new List<Action<Event>>();
                this.subscribers.Add(eventType, actions);
            }
            Action<Event> eventAction = (x) => action.Invoke(x as TEvent); // TODO: HACK and unknown if this works
            actions.Add(eventAction);            
        }

        /// <summary>
        /// Check the validity of an event (i.e. all required fields are set). 
        /// </summary>
        /// <param name="event">Event to check</param>
        /// <exception cref="IncompleteEventException">The domain event is incomplete/invalid and cannot be added to the event store.</exception>
        private void CheckValidity(Event @event)
        {
            if (!@event.IsValid())
            {
                throw new IncompleteEventException();
            }
        }

        ///// <summary>
        ///// Notify subscribers of new events
        ///// </summary>
        ///// <param name="newEvents">New events in ascending order</param>
        //private void NotifySubscribers(IEnumerable<Event> newEvents)
        //{
        //    foreach(var newEvent in newEvents)
        //    {
        //        this.NotifySubscribers(newEvent);
        //    }
        //}

        /// <summary>
        /// Notify subscribers of new event
        /// </summary>
        /// <param name="newEvent">new event</param>
        private void NotifySubscribers<TEvent>(TEvent newEvent) where TEvent : Event
        {
            var eventType = typeof(TEvent);
            if (this.subscribers.TryGetValue(eventType, out var subscriptions))
            {
                foreach (var subscriber in subscriptions)
                {
                    subscriber.Invoke(newEvent);
                }
            }
        }

        /// <summary>
        /// Apply the current vector clock to the event (and increment it)
        /// </summary>
        /// <param name="newEvent">Event to raise and handle</param>
        private void ApplyVectorClock(Event newEvent)
        {
            this.deviceVectorClock.Increment();
            newEvent.VectorClock = this.deviceVectorClock.GetVectorClock();
        }
    }
}
