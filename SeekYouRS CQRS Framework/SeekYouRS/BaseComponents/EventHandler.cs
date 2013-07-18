using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class of an EventHandler.
	/// Any Event who should handle needs a HandleThis method.
	/// </summary>
	public abstract class EventHandler : IHandleAggregateEvents
	{
		/// <summary>
		/// This is a Reference to the concrete ReadModelStore.
		/// </summary>
		public IStoreAndRetrieveReadModels ReadModelStore { get; set; }

		/// <summary>
		/// Calls the concrete HandleThis Method of derived class.
		/// </summary>
		/// <param name="aggregateEvent">The AggregateEventd who should handle</param>
		public void Handle(IAmAnAggregateEvent aggregateEvent)
		{
			HandleThis((dynamic)aggregateEvent);
		}

		/// <summary>
		/// This is the fallback method to inform that an Event could not handle.
		/// </summary>
		/// <param name="aggregateEvent"></param>
		public void HandleThis(object aggregateEvent)
		{
			throw new ArgumentException("Unknown Event detected: " 
				+ aggregateEvent.GetType().Name
				+ " You should provide a HandleThis Method for this Event");
		}
	}
}