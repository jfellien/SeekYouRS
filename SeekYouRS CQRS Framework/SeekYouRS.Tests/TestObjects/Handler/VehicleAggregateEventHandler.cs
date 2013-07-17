using SeekYouRS.BaseComponents;
using SeekYouRS.Contracts;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class VehicleAggregateEventHandler : AggregateEventHandler
	{

		public override void Handle(IAmAnAggregateEvent aggregateEvent)
		{
			Handle((dynamic)aggregateEvent); 
		}

		void Handle(CustomerCreated customerCreated)
		{
			ReadModelStore.Add(new VehicleBaseOfferForNewCustomers{ CustomerId = customerCreated.Id });
		}
	}
}