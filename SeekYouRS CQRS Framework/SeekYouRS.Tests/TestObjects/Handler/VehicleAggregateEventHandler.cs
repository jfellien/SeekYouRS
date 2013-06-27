using SeekYouRS.Handler;
using SeekYouRS.Store;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class VehicleAggregateEventHandler : AggregateEventsHandler
	{
		public VehicleAggregateEventHandler(IStoreAndQueryReadModels readModelStore) : base(readModelStore)
		{
		}

		public override void SaveChangesBy(AggregateEvent aggregateEvent)
		{
			Handle((dynamic)aggregateEvent);
		}

		void Handle(AggregateEventBag<CustomerCreated> customerCreated)
		{
			ReadModelStore.Add(new VehicleBaseOfferForNewCustomers{ CustomerId = customerCreated.EventData.Id });
		}
	}
}