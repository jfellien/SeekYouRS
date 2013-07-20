using SeekYouRS.BaseComponents;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerCommands : CommandHandler
	{
		public override void Handle(dynamic command)
		{
			HandleThis(command);
		}

		public override TResult Handle<TResult>(dynamic command)
		{
			return HandleThis(command);
		}

		void HandleThis(CreateCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Create(command.Id, command.Name);
			AggregateStore.Save(customer);
		}

		void HandleThis(CreateCustomerWithoutAggregatStore command)
		{
			var customer = new Customer();
			customer.Create(command.Id, command.Name);
			AggregateStore.Save(customer);
		}

		void HandleThis(ChangeCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Change(command.Name);
			AggregateStore.Save(customer);
		}

		void HandleThis(RemoveCustomer command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Remove();
			AggregateStore.Save(customer);
		}

		void HandleThis(CommandWithoutEventHandling command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.RaiseUnhandledEvent();
			AggregateStore.Save(customer);
		}

		string HandleThis(GetStringResult command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			return customer.GetStringResult(command.ExpectedResult);
		}

		int HandleThis(GetIntResult command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			return customer.GetIntResult(command.ExpectedResult);
		}

		void HandleThis(StartLongRunningProcess command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.StartLongRunningProcess(command.Milliseconds);
		}
	}
}