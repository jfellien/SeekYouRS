using System;
using System.Collections.Generic;
using SeekYouRS.EventStore;

namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Describes the metghodes of an AggregateEvent Store
	/// </summary>
	public interface IStoreAndRetrieveAggregateEvents
	{
		/// <summary>
		/// Stored an AggregateEvent wrapped with an AggregateEventBag
		/// </summary>
		/// <param name="aggregateEventBag">This Bag adds meta data to an AggregateEvent</param>
		void Store(AggregateEventBag aggregateEventBag);

		/// <summary>
		/// Gets a list of AggregateEventBags by an AggregateId
		/// </summary>
		/// <param name="aggregateId"></param>
		/// <returns></returns>
		IEnumerable<AggregateEventBag> RetrieveBy(Guid aggregateId);
	}
}