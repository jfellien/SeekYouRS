using SeekYouRS.BaseComponents;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerEventHandler : EventHandler
	{
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