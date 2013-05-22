using System;

namespace SeekYouRS.EventSource
{
    public class Event
    {
        public Guid Id { get; set; }
    }

    public class EventBag<T> : Event
    {
        public T EventData { get; set; }
    }
}