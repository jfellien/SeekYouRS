using System;
using System.Linq;
using SeekYouRS.BaseComponents;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS
{
	public class AggregateStore
	{
		readonly EventRecorder _eventRecorder;

		public event Action<IAmAnAggregateEvent> AggregateHasChanged;

		public AggregateStore(EventRecorder eventRecorder)
		{
			_eventRecorder = eventRecorder;
			_eventRecorder.EventHasStored += OnAggregateStateHasChange;
		}

		void OnAggregateStateHasChange(IAmAnAggregateEvent aggregateEvent)
		{
			if (AggregateHasChanged != null)
				AggregateHasChanged(aggregateEvent);
		}

		public void Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
		{
			var aggregateChanges = aggregate.Changes.ToList();

			aggregateChanges.ForEach(_eventRecorder.Record);

			aggregate.History = aggregate.History.Concat(aggregate.Changes).ToList();
			aggregate.Changes.Clear();
		}
		
		public TAggregate GetAggregate<TAggregate>(Guid id) where TAggregate : Aggregate, new()
		{
			var aggregateHistory = _eventRecorder.ReplayFor(id).ToList();
			var aggregate = new TAggregate
				{
					History = aggregateHistory
				};
			return aggregate;
		}
	}
}