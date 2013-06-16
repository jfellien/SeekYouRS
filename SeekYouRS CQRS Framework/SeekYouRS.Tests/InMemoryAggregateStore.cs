using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Store;

namespace SeekYouRS.Tests
{
	class InMemoryAggregateStore : IAmAnAggregatesUnitOfWork
	{
		private readonly List<AggregateEvent> _worldhistory;

		public InMemoryAggregateStore()
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