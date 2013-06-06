using System;

namespace SeekYouRS.Storing
{
    /// <summary>
    /// Represents an Aggregate Event. It will use as an identifier of state changes
    /// </summary>
    public class AggregateEvent
    {
        public AggregateEvent(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }
}