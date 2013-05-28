using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Storing;

namespace SeekYouRS.Tests
{
    class InMemoryAggregateStore : AggregateStore
    {
        public event Action<AggregateEvent> Publish;

        private readonly List<AggregateEvent> _worldhistory;

        public InMemoryAggregateStore()
        {
            _worldhistory = new List<AggregateEvent>();
        }

        public void Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
        {
            _worldhistory.AddRange(aggregate.Changes);

            if(Publish != null)
                foreach (var e in aggregate.Changes) 
                    Publish(e);

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