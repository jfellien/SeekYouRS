using System;
using System.Collections.Generic;

namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Represents the behaviour of a AggregateStore
	/// </summary>
	public interface IStoreAndRetrieveAggregateEvents
	{
		/// <summary>
		/// Saves a list of AggregateEvents who describes the changes
		/// </summary>
		/// <param name="changes">List of changes</param>
		void Save(IEnumerable<AggregateEvent> changes);
		
		/// <summary>
		/// Gets the list of AggregateEvents by Id of Aggregate
		/// </summary>
		/// <param name="id">Id of Aggregate</param>
		/// <returns>List of AggregateEvents</returns>
		IEnumerable<AggregateEvent> GetEventsBy(Guid id);
	}
}