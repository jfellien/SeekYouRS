using System;

namespace SeekYouRS.Storing
{
    /// <summary>
    /// Represents an extension of AggregateEvent. It contains the concrete data of a status change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AggregateEventBag<T> : AggregateEvent
    {
        public AggregateEventBag(Guid id) : base(id)
        {}

        /// <summary>
        /// The data of status change.
        /// </summary>
        public T EventData { get; set; }
    }
}