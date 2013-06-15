using SeekYouRS.Store;

namespace SeekYouRS.Handler
{
	/// <summary>
	/// Handles changes for ReadModels and execute Queries to get this ReadModels
	/// </summary>
	public class ReadModelHandler
	{
		readonly IStoreAggregateEventsAsReadModels _readModelStore;

		/// <summary>
		/// Constructor of instance
		/// </summary>
		/// <param name="readModelStore">Implementation of specific ReadModelStore</param>
		public ReadModelHandler(IStoreAggregateEventsAsReadModels readModelStore)
		{
			_readModelStore = readModelStore;
		}

		/// <summary>
		/// Exececutes a query and returns a expected instance of T
		/// </summary>
		/// <typeparam name="T">Expected type of Model. Lists and single Modles allowed</typeparam>
		/// <param name="query">Instance of query parameters</param>
		/// <returns></returns>
		public T Execute<T>(dynamic query)
		{
			return (T)_readModelStore.Retrieve<T>(query);
		}

		/// <summary>
		/// Applies changes described by an AggregateEvent
		/// </summary>
		/// <param name="aggregateEvent">Changes description</param>
		public void ApplyChanges(AggregateEvent aggregateEvent)
		{
			_readModelStore.SaveChangesBy(aggregateEvent);
		}
	}
}
