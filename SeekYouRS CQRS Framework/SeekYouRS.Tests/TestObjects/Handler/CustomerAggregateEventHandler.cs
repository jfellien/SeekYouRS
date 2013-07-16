using SeekYouRS.Contracts;
using SeekYouRS.EventStore;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerAggregateEventHandler : AggregateEventHandler
	{
		public CustomerAggregateEventHandler(IStoreAndRetrieveReadModels readModelStore) 
		: base(readModelStore)
		{
		}

		public override void Handle(AggregateEvent aggregateEvent)
		{
			HandleAggregateEvent((dynamic)aggregateEvent);
		}

		void HandleAggregateEvent(AggregateEventBag<CustomerCreated> customerCreated)
		{
			ReadModelStore.Add(new CustomerModel
				{
					Id = customerCreated.EventData.Id,
					Name = customerCreated.EventData.Name
				});
		}

		void HandleAggregateEvent(AggregateEventBag<CustomerChanged> customerChanged)
		{
			ReadModelStore.Change(new CustomerModel
				{
					Id = customerChanged.Id,
					Name = customerChanged.EventData.Name
				});
		}

		void HandleAggregateEvent(AggregateEventBag<CustomerRemoved> customerRemoved)
		{
			ReadModelStore.Remove(new CustomerModel { Id = customerRemoved.Id });
		}
	}
}