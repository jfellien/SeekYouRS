using Sample.CQRS.Contracts.Events;
using Sample.CQRS.Contracts.Models;
using SeekYouRS.Contracts;

namespace Sample.CQRS.ReadModel.EventHandler
{
	public class CustomerEventHandler : SeekYouRS.BaseComponents.EventHandler
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
	}
}