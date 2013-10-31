using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Document;
using Raven.Client.Linq;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace Sample.CQRS.Infrastructure
{
    public class RavenDBAggregateEventStore : IStoreAndRetrieveAggregateEvents
    {
        readonly DocumentStore _store;

        public RavenDBAggregateEventStore()
        {
            _store = new DocumentStore
            {
                ConnectionStringName = "MyConnection"
            };
            _store.Initialize();
        }

        public void Store(AggregateEventBag aggregateEventBag)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(aggregateEventBag);
                session.SaveChanges();
            }
        }

        public IEnumerable<AggregateEventBag> RetrieveBy(Guid aggregateId)
        {
            IEnumerable<AggregateEventBag> allAggregateEvents;

            using (var session = _store.OpenSession())
            {
                var query = from bag in session.Query<AggregateEventBag>()
                            where bag.AggregateId.Equals(aggregateId)
                            select bag;

                allAggregateEvents = query.ToList();
            }

            return allAggregateEvents;
        }
    }
}