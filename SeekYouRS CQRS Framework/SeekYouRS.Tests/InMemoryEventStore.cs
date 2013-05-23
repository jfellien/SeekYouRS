using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.EventSource;

namespace SeekYouRS.Tests
{
    class InMemoryEventStore : IStoreEvents
    {
        private readonly List<Event> _worldhistory;

        public InMemoryEventStore()
        {
            _worldhistory = new List<Event>();
        }

        public void Store(IList<Event> eventData)
        {
            _worldhistory.AddRange(eventData);

            // Keine Ahnung, ob ist das hier lasse
            foreach (var e in eventData) 
                OnNewEvent(e);
        }

        public IEnumerable<Event> GetAggregateHistoryBy(Guid id)
        {
            return _worldhistory.Where(@event => @event.Id == id).ToList();
        }

        public IEnumerable<Event> EventHistory { get { return _worldhistory.ToList(); } }


        // Weiﬂ noch nicht wie ich damit umgehe
        public void Subscribe(Action<Event> observer)
        {
            NewEvent += observer;
        }

        private event Action<Event> NewEvent;

        protected virtual void OnNewEvent(Event obj)
        {
            Action<Event> handler = NewEvent;
            if (handler != null) handler(obj);
        }
    }
}