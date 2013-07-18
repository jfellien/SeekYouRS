using SeekYouRS.BaseComponents;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class VehicleEventHandler : EventHandler
	{
		void HandleThis(CustomerCreated customerCreated)
		{
			ReadModelStore.Add(new VehicleBaseOfferForNewCustomers{ CustomerId = customerCreated.Id });
		}
	}
}