using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Store;

namespace SeekYouRS.Tests
{
    class InMemoryAggregateStore : IStoreAggregates
    {
        public event Action<AggregateEvent> AggregateHasChanged;

        private readonly List<AggregateEvent> _worldhistory;

        public InMemoryAggregateStore()
        {
            _worldhistory = new List<AggregateEvent>();
        }

        public void Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
        {
            _worldhistory.AddRange(aggregate.Changes);

            if(AggregateHasChanged != null)
                foreach (var e in aggregate.Changes) 
                    AggregateHasChanged(e);

            aggregate.History = aggregate.History.Concat(aggregate.Changes).ToList();
            aggregate.Changes.Clear();
        }

        public TAggregate GetAggregate<TAggregate>(Guid id) where TAggregate : Aggregate, new()
        {
            var aggregateHistory = _worldhistory.Where(@event => @event.Id == id).ToList();
            var aggregate = new TAggregate
                {
                    History = aggregateHistory
                };
            return aggregate;
        }
        
    }
}