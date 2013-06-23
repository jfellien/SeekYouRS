using SeekYouRS.Handler;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerCommands : CommandHandler
	{
		public CustomerCommands(Store.IStoreAndRetrieveAggregates aggregatesStore) 
		: base(aggregatesStore) { }

		public override void Process(dynamic command)
		{
			Execute(command);
		}

		private void Execute(CreateCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Create(command.Id, command.Name);
			AggregateStore.Save(customer);
		}

		private void Execute(ChangeCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Change(command.Name);
			AggregateStore.Save(customer);
			
		}

		private void Execute(RemoveCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Remove();
			AggregateStore.Save(customer);
		}

		private void Execute(CommandWithoutEventHandling command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.RaiseUnhandledEvent();
			AggregateStore.Save(customer);
		}
	}
}