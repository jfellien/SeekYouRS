using SeekYouRS.Handler;
using SeekYouRS.Store;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerAggregateEventHandler : AggregateEventsHandler
	{
		public CustomerAggregateEventHandler(IStoreAndQueryReadModels readModelStore) : base(readModelStore)
		{
		}

		public override void SaveChangesBy(AggregateEvent aggregateEvent)
		{
			Handle((dynamic)aggregateEvent);
		}

		void Handle(AggregateEventBag<CustomerCreated> customerCreated)
		{
			ReadModelStore.Add(new CustomerModel
				{
					Id = customerCreated.EventData.Id,
					Name = customerCreated.EventData.Name
				});
		}

		void Handle(AggregateEventBag<CustomerChanged> customerChanged)
		{
			ReadModelStore.Change(new CustomerModel
				{
					Id = customerChanged.Id,
					Name = customerChanged.EventData.Name
				});
		}

		void Handle(AggregateEventBag<CustomerRemoved> customerRemoved)
		{
			ReadModelStore.Remove(new CustomerModel { Id = customerRemoved.Id });
		}
	}
}