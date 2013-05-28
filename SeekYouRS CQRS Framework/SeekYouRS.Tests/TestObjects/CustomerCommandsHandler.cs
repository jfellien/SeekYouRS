using System;
using SeekYouRS.Messaging;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects.Commands;

namespace SeekYouRS.Tests.TestObjects
{
    public class CustomerCommandsHandler : IExecuteCommands
    {
        public event Action<AggregateEvent> Performed;

        private readonly AggregateStore _aggregateEventStore;

        public CustomerCommandsHandler(AggregateStore aggregateEventStore)
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

        private void Execute(ErfasseKunde command)
        {
            var kunde = _aggregateEventStore.GetAggregate<Customer>(command.Id);
            kunde.Create(command.Id, command.Name);
            _aggregateEventStore.Save(kunde);
        }

        private void Execute(ÄndereKunde command)
        {
            var kunde = _aggregateEventStore.GetAggregate<Customer>(command.Id);
            kunde.Change(command.Name);
            _aggregateEventStore.Save(kunde);
            
        }
    }
}