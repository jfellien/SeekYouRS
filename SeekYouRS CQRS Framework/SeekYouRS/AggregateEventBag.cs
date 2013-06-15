using System;

namespace SeekYouRS
{
	/// <summary>
	/// Represents an extension of AggregateEvent. It contains the concrete data of a status change.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AggregateEventBag<T> : AggregateEvent
	{
		/// <summary>
		/// Constructor of instance
		/// </summary>
		/// <param name="id">Id of Instance</param>
		/// <param name="timestamp">Time when Event raised</param>
		public AggregateEventBag(Guid id, DateTime timestamp) : base(id)
		{
			Timestamp = timestamp;
		}

		/// <summary>
		/// Gets the Timestamp of event
		/// </summary>
		public DateTime Timestamp { get; private set; }

		/// <summary>
		/// The data of event
		/// </summary>
		public T EventData { get; set; }
	}
}