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

		private string HandleThis(GetStringResult command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			return customer.GetStringResult(command.ExpectedResult);
		}

		private int HandleThis(GetIntResult command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			return customer.GetIntResult(command.ExpectedResult);
		}
	}
}