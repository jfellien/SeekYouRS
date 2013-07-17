using System;
using System.Collections.Generic;
using SeekYouRS.EventStore;

namespace SeekYouRS.Contracts
{
	public interface IStoreAndRetrieveAggregateEvents
	{
		void Store(AggregateEventBag aggregateEventBag);
		IEnumerable<AggregateEventBag> RetrieveBy(Guid aggregateId);
	}
}