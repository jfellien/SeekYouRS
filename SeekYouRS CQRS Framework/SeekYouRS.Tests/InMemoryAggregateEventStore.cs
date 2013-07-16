using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.Tests
{
	class InMemoryAggregateEventStore : IStoreAndRetrieveAggregateEvents
	{
		private readonly List<AggregateEvent> _worldhistory;

		public InMemoryAggregateEventStore()
		{
			_worldhistory = new List<AggregateEvent>();
		}

		public void Save(IEnumerable<AggregateEvent> changes)
		{
			_worldhistory.AddRange(changes);
		}

		public IEnumerable<AggregateEvent> GetEventsBy(Guid id)
		{
			return _worldhistory.Where(@event => @event.Id == id);
		}
	}
}