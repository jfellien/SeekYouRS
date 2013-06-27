using SeekYouRS.Contracts;
using SeekYouRS.Store;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class VehicleAggregateEvents : AggregateEvents
	{
		public VehicleAggregateEvents(IStoreAndRetrieveReadModels readModelStore) : base(readModelStore)
		{
		}

		public override void SaveChangesBy(AggregateEvent aggregateEvent)
		{
			Save((dynamic)aggregateEvent);
		}

		void Handle(AggregateEventBag<CustomerCreated> customerCreated)
		{
			ReadModelStore.Add(new VehicleBaseOfferForNewCustomers{ CustomerId = customerCreated.EventData.Id });
		}
	}
}