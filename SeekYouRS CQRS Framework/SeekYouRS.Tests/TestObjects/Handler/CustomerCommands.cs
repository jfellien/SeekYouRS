using System;
using System.Threading.Tasks;
using SeekYouRS.BaseComponents;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects.Handler
{
	public class CustomerCommands : CommandHandler
	{
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

		private void HandleThis(LongRunningCommand command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.CallLongRunningMethod();
		}

		private String HandleThis(GetStringResult command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			var result = customer.GetStringResult(command.Name);
			AggregateStore.Save(customer);

			return result;
		}

		private int HandleThis(GetIntResult command)
		{
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			var result = customer.GetIntResult(command.Number);
			AggregateStore.Save(customer);

			return result;
		}
	}
}