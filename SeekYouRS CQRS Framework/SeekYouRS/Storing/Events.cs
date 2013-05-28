using System;
using System.Collections.Generic;

namespace SeekYouRS.Storing
{
    public class Events
    {
        private readonly IStoreAggregateEvents _aggregateEventStore;

        public Events(IStoreAggregateEvents aggregateEventStore)
        {
            _aggregateEventStore = aggregateEventStore;
        }

        public void SaveFor<TAggregeate>(TAggregeate aggregate) where TAggregeate : Aggregate
        {
            _aggregateEventStore.Save(aggregate);
            aggregate.Changes.Clear();
        }

        public IEnumerable<AggregateEvent> GetHistoryOf<TAggregate>(Guid aggregateId) where TAggregate : Aggregate, new()
        {
            return _aggregateEventStore.GetAggregate<TAggregate>(aggregateId).History;
        }
    }
}
