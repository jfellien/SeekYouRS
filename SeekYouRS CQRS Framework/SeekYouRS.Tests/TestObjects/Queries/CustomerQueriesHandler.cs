using System;
using System.Diagnostics;
using SeekYouRS.Messaging;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects.Events;

namespace SeekYouRS.Tests.TestObjects.Queries
{
    public class CustomerQueriesHandler : QueriesRepository
    {
        private readonly QueriesStore _queriesStore;

        public CustomerQueriesHandler(QueriesStore queriesStore)
        {
            _queriesStore = queriesStore;
        }

        public T Execute<T>(dynamic query)
        {
            return default(T);  //_queriesStore.Retrieve<T>(query);
        }

        public void HandleChanges(AggregateEvent aggregateEvent)
        {
            Handle((dynamic)aggregateEvent);
        }

        void Handle(object unassignedEvent)
        {
            throw new ArgumentException("This event is not assigned to this instance, " unassignedEvent.GetType().Name);
        }

        void Handle(AggregateEventBag<KundeWurdeErfasst> kundeWurdeErfasst)
        {
            Trace.WriteLine("Kunde wurde erfasst");
        }
    }
}