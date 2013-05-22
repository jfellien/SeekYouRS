using System;

namespace SeekYouRS.EventSource
{
    public class Event
    {
        public Event(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }

    public class EventBag<T> : Event
    {
        public EventBag(Guid id) : base(id)
        {}

        public T EventData { get; set; }
    }
}