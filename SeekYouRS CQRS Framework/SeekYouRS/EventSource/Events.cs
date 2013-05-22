using System;
using System.Collections.Generic;

namespace SeekYouRS.EventSource
{
    public class Events
    {
        private readonly IStoreEvents _eventStore;

        public Events(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public void SaveFor<TAggregeate>(TAggregeate aggregate) where TAggregeate : Aggegate
        {
            _eventStore.Store(aggregate.Changes);
            aggregate.Changes.Clear();
        }

        public IEnumerable<Event> GetHistoryOf(Guid aggregateId)
        {
            return _eventStore.GetAggregateHistoryBy(aggregateId);
        }
    }
}
