using System;
using System.Collections.Generic;

namespace SeekYouRS.Store
{
	public interface IAmAnAggregatesUnitOfWork
	{
		void Save(IEnumerable<AggregateEvent> changes);
		IEnumerable<AggregateEvent> GetEventsBy(Guid id);
	}
}