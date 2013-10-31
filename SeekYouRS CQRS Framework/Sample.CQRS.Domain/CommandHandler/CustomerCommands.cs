using Sample.CQRS.Contracts.Commands;
using Sample.CQRS.Domain.Aggregates;

namespace Sample.CQRS.Domain.CommandHandler
{
    public class CustomerCommands : SeekYouRS.BaseComponents.CommandHandler
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

        void HandleThis(ChangeCustomer command)
        {
            var customer = AggregateStore.GetAggregate<Customer>(command.Id);
            customer.Change(command.Name);
            AggregateStore.Save(customer);
        }
    }
}