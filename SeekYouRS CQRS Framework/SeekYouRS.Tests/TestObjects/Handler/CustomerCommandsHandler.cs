using System;
using SeekYouRS.Messaging;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects.Handler
{
    public class CustomerCommandsHandler : IExecuteCommands
    {
        public event Action<AggregateEvent> Performed;

        private readonly IStoreAggregates _aggregateEventStore;

        public CustomerCommandsHandler(IStoreAggregates aggregateEventStore)
        {
            _aggregateEventStore = aggregateEventStore;
            _aggregateEventStore.Publish += OnEventPublished ;
        }

        private void OnEventPublished(AggregateEvent aggregateEvent)
        {
            if (Performed != null)
                Performed(aggregateEvent);
        }

        public void Process(dynamic command)
        {
            Execute(command);
        }

        private void Execute(object command)
        {
            throw new ArgumentException("Unnown Command detected: " + command.GetType().Name);
        }

        private void Execute(CreateCustomer command)
        {
            var customer = _aggregateEventStore.GetAggregate<Customer>(command.Id);
            customer.Create(command.Id, command.Name);
            _aggregateEventStore.Save(customer);
        }

        private void Execute(ChangeCustomer command)
        {
            var customer = _aggregateEventStore.GetAggregate<Customer>(command.Id);
            customer.Change(command.Name);
            _aggregateEventStore.Save(customer);
            
        }

        private void Execute(RemoveCustomer command)
        {
            var customer = _aggregateEventStore.GetAggregate<Customer>(command.Id);
            customer.Remove();
            _aggregateEventStore.Save(customer);
        }
    }
}