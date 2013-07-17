using SeekYouRS.BaseComponents;
using SeekYouRS.Contracts;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerAggregateEventHandler : AggregateEventHandler
	{
		public override void Handle(IAmAnAggregateEvent aggregateEvent)
		{
			HandleThis((dynamic)aggregateEvent);
		}

		void HandleThis(CustomerCreated customerCreated)
		{
			ReadModelStore.Add(new CustomerModel
				{
					Id = customerCreated.Id,
					Name = customerCreated.Name
				});
		}

		void HandleThis(CustomerChanged customerChanged)
		{
			ReadModelStore.Change(new CustomerModel
				{
					Id = customerChanged.Id,
					Name = customerChanged.Name
				});
		}

		void HandleThis(CustomerRemoved customerRemoved)
		{
			ReadModelStore.Remove(new CustomerModel { Id = customerRemoved.Id });
		}
	}
}