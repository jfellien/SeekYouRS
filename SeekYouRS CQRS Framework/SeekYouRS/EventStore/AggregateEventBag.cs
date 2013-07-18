using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.EventStore
{
	/// <summary>
	/// Wraps an AggregateEvent anbd added meta information to this Event
	/// </summary>
	public class AggregateEventBag
	{
		/// <summary>
		/// Gets or sets the Id of assigned Aggregate
		/// </summary>
		public Guid AggregateId { get; set; }
		/// <summary>
		/// Gets or sets the Time who saved the AggregateEvent
		/// </summary>
		public DateTime TimeStamp { get; set; }
		/// <summary>
		/// Gets or sets the concrete Event
		/// </summary>
		public IAmAnAggregateEvent EventData { get; set; }
	}
}