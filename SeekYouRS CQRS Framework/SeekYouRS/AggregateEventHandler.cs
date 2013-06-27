using System;
using SeekYouRS.Contracts;

namespace SeekYouRS
{
	/// <summary>
	/// Base class for AggregateEventHandler. 
	/// This handles AggregateEvents and gets the ReadModelStore setted by constructor.
	/// </summary>
	public abstract class AggregateEventHandler : IHandleAggregateEvents
	{
		protected AggregateEventHandler(IStoreAndRetrieveReadModels unitOfWork)
		{
			ReadModelStore = unitOfWork;
		}

		public IStoreAndRetrieveReadModels ReadModelStore { get; private set; }

		/// <summary>
		/// Derived method from interface IHandleAggregateEvents. 
		/// You should implement this method only with calling 'HandleAggregateEvent(aggregateEvent)' 
		/// and implement all your AggregateEvent Handler with own method 
		/// 'private void HandleAggregateEvent(YorAggregateEvent aggregateEvent)'
		/// </summary>
		/// <param name="aggregateEvent">The Event who will handled and contains the change data</param>
		public abstract void Handle(AggregateEvent aggregateEvent);

		/// <summary>
		/// Fallback method to handle unassigned AggregateEvent
		/// </summary>
		/// <param name="unassignedEvent"></param>
		public void HandleAggregateEvent(object unassignedEvent)
		{
			var eventData = ((dynamic) unassignedEvent).EventData;
			var eventTypeName = eventData.GetType().Name;
			throw new ArgumentException(
				String.Format(
					"This event is not assigned to this instance, {0}\r\n"
					+ "Be sure you have write a methode like void HandleAggregateEvent(AggregateBag<{0}> aggregateEvent)",
					eventTypeName));
		}
	}
}