using System;
using System.Collections.Generic;

namespace SeekYouRS.Store
{
	public interface IStoreAndRetrieveAggregates
	{
		void Save(IEnumerable<AggregateEvent> changes);
		IEnumerable<AggregateEvent> GetEventsBy(Guid id);
	}
}