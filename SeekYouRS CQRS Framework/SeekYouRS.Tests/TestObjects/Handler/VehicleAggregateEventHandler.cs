using SeekYouRS.Contracts;
using SeekYouRS.EventStore;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class VehicleAggregateEventHandler : AggregateEventHandler
	{
		public VehicleAggregateEventHandler(IStoreAndRetrieveReadModels readModelStore) : base(readModelStore)
		{
		}

		public override void Handle(AggregateEvent aggregateEvent)
		{
			Handle((dynamic)aggregateEvent); 
		}

		void Handle(AggregateEventBag<CustomerCreated> customerCreated)
		{
			ReadModelStore.Add(new VehicleBaseOfferForNewCustomers{ CustomerId = customerCreated.EventData.Id });
		}
	}
}