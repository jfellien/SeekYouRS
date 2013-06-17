using System;
using SeekYouRS.Store;

namespace SeekYouRS.Handler
{
	public abstract class ReadModelHandler
	{
		protected ReadModelHandler(IStoreReadModels readModelStore)
		{
			ReadModelStore = readModelStore;
		}

		public IStoreReadModels ReadModelStore { get; private set; }

		public abstract void SaveChangesBy(AggregateEvent aggregateEvent);

		internal void Handle(object unassignedEvent)
		{
			var eventData = ((dynamic)unassignedEvent).EventData;

			throw new ArgumentException("This event is not assigned to this instance, " + eventData.GetType().Name);
		}
	}
}