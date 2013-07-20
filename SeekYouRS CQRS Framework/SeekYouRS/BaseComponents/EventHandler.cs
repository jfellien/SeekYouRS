using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class of an EventHandler.
	/// Any Event who should handle needs a HandleThis method.
	/// </summary>
	public abstract class EventHandler
	{
		/// <summary>
		/// This is a Reference to the concrete ReadModelStore.
		/// </summary>
		public IStoreAndRetrieveReadModels ReadModelStore { get; set; }

		/// <summary>
		/// This method calls the concrete implementation of Handling Method.
		/// You should implement this method by calling 'HandleThis(aggregateEvent)' and 
		/// implement for any Command an HandleThis method.
		/// </summary>
		/// <param name="aggregateEvent">The Event who will handle</param>
		public abstract void Handle(IAmAnAggregateEvent aggregateEvent);

		/// <summary>
		/// This is the fallback method to inform that an Event could not handle.
		/// </summary>
		/// <param name="aggregateEvent"></param>
		public void HandleThis(object aggregateEvent)
		{
			throw new ArgumentException("Unnown Event detected: " + aggregateEvent.GetType().Name);
		}
	}
}