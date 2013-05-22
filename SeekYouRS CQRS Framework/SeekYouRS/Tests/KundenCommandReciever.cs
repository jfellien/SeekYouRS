using System;
using SeekYouRS.EventSource;
using SeekYouRS.Messaging;

namespace SeekYouRS.Tests
{
    public class KundenCommandReciever : IRecieveCommands
    {
        private readonly IStoreEvents _eventStore;

        public KundenCommandReciever(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }
        public void Recieve(dynamic command)
        {
            Execute(command);
        }

        private void Execute(object command)
        {
            throw new ArgumentException(command.GetType().Name);
        }

        private void Execute(ErfasseKunde command)
        {
            var kunde = GetAggregateFor<Kunde>(command.Id);
            kunde.Create(command.Id, command.Name);
            SaveAggregate(kunde);
        }

        private void SaveAggregate<T>(T aggregate) where T : Aggegate
        {
            _eventStore.Store(aggregate.Changes);
        }

        private T GetAggregateFor<T>(Guid aggregateId) where T : Aggegate, new()
        {
            var eventHistory = _eventStore.GetAggregateHistoryBy(aggregateId);
            var aggregate = new T {History = eventHistory};

            return aggregate;
        }
    }
}