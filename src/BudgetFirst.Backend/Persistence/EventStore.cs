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
    public class EventStore : IEventStore
    {
        /// <summary>
        /// All saved events
        /// </summary>
        private EventStoreState state; // = new EventStoreState();
       
        public EventStore(EventStoreState state)
        {
            this.state = state;
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

        /// <summary>
        /// Save multiple events
        /// </summary>
        /// <param name="domainEvents">Events to save</param>
        public void Add(IEnumerable<Event> domainEvents)
        {
            var newEvents = domainEvents.ToList();
            foreach (var @event in newEvents)
            {
                this.CheckValidity(@event);    
            }

            this.state.Events.AddRange(newEvents);
            this.NotifySubscribers(newEvents);
        }
        
        /// <summary>
        /// Save a single event
        /// </summary>
        /// <param name="domainEvent">Event to save</param>
        public void Add(Event domainEvent)
        {
            if(domainEvent is null)
            {
                throw new ArgumentNullException(nameof(domainEvent));
            }
            this.CheckValidity(domainEvent);
            this.state.Events.Add(domainEvent);
            this.NotifySubscribers(domainEvent);
        }

        // TODO: bad delegate, makes it impossible to remove later
        public void SubscribeTo<TEvent>(Action<TEvent> action) where TEvent : Event
        {
            // TODO: implement. 
            throw new NotImplementedException();
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

        /// <summary>
        /// Notify subscribers of new events
        /// </summary>
        /// <param name="newEvents">New events in ascending order</param>
        private void NotifySubscribers(IEnumerable<Event> newEvents)
        {
            foreach(var newEvent in newEvents)
            {
                this.NotifySubscribers(newEvent);
            }
        }

        /// <summary>
        /// Notify subscribers of new event
        /// </summary>
        /// <param name="newEvent">new event</param>
        private void NotifySubscribers(Event newEvent)
        {
            // TODO: implement. 
            throw new NotImplementedException();
        }
    }
}
