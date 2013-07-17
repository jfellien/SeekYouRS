using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.EventStore
{
	public class AggregateEventBag
	{
		public Guid AggregateId { get; set; }

		public DateTime TimeStamp { get; set; }

		public IAmAnAggregateEvent EventData { get; set; }
	}
}