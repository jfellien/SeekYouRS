using System;
using SeekYouRS.Handler;
using SeekYouRS.Store;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects.Handler
{
    public class CustomerCommands : IExecuteCommands
    {
        public event Action<AggregateEvent> HasPerformed;

        private readonly IStoreAggregates _aggregateEventStore;

        public CustomerCommands(IStoreAggregates aggregateEventStore)
        {
            _aggregateEventStore = aggregateEventStore;
            _aggregateEventStore.AggregateHasChanged += OnEventPublished ;
        }

        private void OnEventPublished(AggregateEvent aggregateEvent)
        {
            if (HasPerformed != null)
                HasPerformed(aggregateEvent);
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
            var customer =_aggregateEventStore.GetAggregate<Customer>(command.Id);
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