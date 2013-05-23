using System;
using System.Collections.Generic;

namespace SeekYouRS.EventSource
{
    public interface IStoreEvents
    {
        void Store(IList<Event> events);
        IEnumerable<Event> GetAggregateHistoryBy(Guid aggregateId);
        IEnumerable<Event> EventHistory { get; }
        void Subscribe(Action<Event> observer);
    }
}
