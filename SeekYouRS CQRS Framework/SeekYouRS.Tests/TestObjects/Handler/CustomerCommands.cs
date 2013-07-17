using SeekYouRS.BaseComponents;
using SeekYouRS.EventStore;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerCommands : AggregateCommandHandler
	{
		public override void Handle(dynamic command)
		{
			HandleThis(command);
		}

		private void HandleThis(CreateCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Create(command.Id, command.Name);
			AggregateStore.Save(customer);
		}

		private void HandleThis(ChangeCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Change(command.Name);
			AggregateStore.Save(customer);
			
		}

		private void HandleThis(RemoveCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Remove();
			AggregateStore.Save(customer);
		}

		private void HandleThis(CommandWithoutEventHandling command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.RaiseUnhandledEvent();
			AggregateStore.Save(customer);
		}
	}
}