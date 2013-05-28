using System;

namespace SeekYouRS.Storing
{
    public class AggregateEvent
    {
        public AggregateEvent(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }

    public class AggregateEventBag<T> : AggregateEvent
    {
        public AggregateEventBag(Guid id) : base(id)
        {}

        public T EventData { get; set; }
    }
}