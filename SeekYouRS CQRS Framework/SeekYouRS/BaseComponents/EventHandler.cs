using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.BaseComponents
{
	public abstract class EventHandler
	{
		public IStoreAndRetrieveReadModels ReadModelStore { get; set; }

		public abstract void Handle(IAmAnAggregateEvent aggregateEvent);

		public void HandleThis(object aggregateEvent)
		{
			throw new ArgumentException("Unnown Event detected: " + aggregateEvent.GetType().Name);
		}

	}
}