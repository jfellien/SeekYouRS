using System;
using SeekYouRS.Store;

namespace SeekYouRS.Handler
{
	/// <summary>
	/// Base class for ReadModelHandler. 
	/// This handles AggregateEvents and gets the ReadModelStore setted by constructor.
	/// </summary>
	public abstract class ReadModelHandler : IHandleAggregateEvents
	{
		protected ReadModelHandler(IStoreReadModels readModelStore)
		{
			ReadModelStore = readModelStore;
		}

		public IStoreReadModels ReadModelStore { get; private set; }

		/// <summary>
		/// Derived method from interface IHandleAggregateEvents. 
		/// You should implement this method only with calling 'Handle(aggregateEvent)' 
		/// and implement all your AggregateEvent Handler with own method 
		/// 'private void Handle(YorAggregateEvent aggregateEvent)'
		/// </summary>
		/// <param name="aggregateEvent">The Event who will handled and contains the change data</param>
		public abstract void SaveChangesBy(AggregateEvent aggregateEvent);

		/// <summary>
		/// Fallback method to handle unassigned AggregateEvent
		/// </summary>
		/// <param name="unassignedEvent"></param>
		public void Handle(object unassignedEvent)
		{
			var eventData = ((dynamic) unassignedEvent).EventData;
			var eventTypeName = eventData.GetType().Name;
			throw new ArgumentException(
				String.Format(
					"This event is not assigned to this instance, {0}\r\n"
					+ "Be sure you have write a methode like void Handle(AggregateBag<{0}> aggregateEvent)",
					eventTypeName));
		}
	}
}