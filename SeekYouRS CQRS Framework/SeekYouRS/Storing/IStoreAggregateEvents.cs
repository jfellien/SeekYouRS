using System;

namespace SeekYouRS.Storing
{
    public interface IStoreAggregateEvents
    {
        void Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate;
        TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : Aggregate, new();
    }
}
