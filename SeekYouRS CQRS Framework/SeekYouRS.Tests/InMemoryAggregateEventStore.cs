using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.Tests
{
	class InMemoryAggregateEventStore : IStoreAndRetrieveAggregateEvents
	{
		private readonly List<AggregateEventBag> _worldhistory;

		public InMemoryAggregateEventStore()
		{
			_worldhistory = new List<AggregateEventBag>();
		}

		public void Store(AggregateEventBag changes)
		{
			_worldhistory.Add(changes);
		}

		public IEnumerable<AggregateEventBag> RetrieveBy(Guid aggregateId)
		{
			return _worldhistory.Where(aggregateEventBag => aggregateEventBag.AggregateId == aggregateId);
		}
	}
}