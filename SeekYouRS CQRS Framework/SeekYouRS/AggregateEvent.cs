using System;

namespace SeekYouRS
{
    /// <summary>
    /// Represents an Aggregate Event. It will use as an identifier of state changes. 
    /// This class will not use direct by developers.
    /// </summary>
    public class AggregateEvent
    {
        /// <summary>
        /// Constructor of Instance
        /// </summary>
        /// <param name="id">Id of Aggregate</param>
        public AggregateEvent(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the ID of current instance
        /// </summary>
        public Guid Id { get; private set; }
    }
}