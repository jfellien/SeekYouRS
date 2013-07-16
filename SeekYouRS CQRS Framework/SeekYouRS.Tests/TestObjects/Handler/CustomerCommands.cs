using SeekYouRS.Contracts;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerCommands : SeekYouRS.Commands
	{
		public CustomerCommands(IStoreAndRetrieveAggregateEvents aggregateEventsStore) 
		: base(aggregateEventsStore) { }

		public override void Execute(dynamic command)
		{
			Handle(command);
		}

		private void Handle(CreateCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Create(command.Id, command.Name);
			AggregateStore.Save(customer);
		}

		private void Handle(ChangeCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Change(command.Name);
			AggregateStore.Save(customer);
			
		}

		private void Handle(RemoveCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Remove();
			AggregateStore.Save(customer);
		}

		private void Handle(CommandWithoutEventHandling command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.RaiseUnhandledEvent();
			AggregateStore.Save(customer);
		}
	}
}