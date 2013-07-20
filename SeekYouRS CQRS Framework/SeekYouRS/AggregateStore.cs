using System;
using System.Linq;
using SeekYouRS.BaseComponents;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS
{
	/// <summary>
	/// This component helps to save and restore Aggregates. 
	/// </summary>
	public sealed class AggregateStore
	{
		readonly EventRecorder _eventRecorder;

		/// <summary>
		/// Raises if an AggregateEvent has change the status of an Aggregate
		/// </summary>
		public event Action<IAmAnAggregateEvent> AggregateHasChanged;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventRecorder">Provides the store and retrieve of AggregateEvents</param>
		public AggregateStore(EventRecorder eventRecorder)
		{
			_eventRecorder = eventRecorder;
			_eventRecorder.EventHasStored += OnAggregateStateHasChange;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aggregateEvent"></param>
		void OnAggregateStateHasChange(IAmAnAggregateEvent aggregateEvent)
		{
			if (AggregateHasChanged != null)
				AggregateHasChanged(aggregateEvent);
		}

		/// <summary>
		/// Saves an Aggregate
		/// </summary>
		/// <typeparam name="TAggregate"></typeparam>
		/// <param name="aggregate"></param>
		public void Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
		{
			var aggregateChanges = aggregate.Changes.ToList();

			aggregateChanges.ForEach(_eventRecorder.Record);

			aggregate.History = aggregate.History.Concat(aggregate.Changes).ToList();
			aggregate.Changes.Clear();
		}

		/// <summary>
		/// Restores an Aggregate by its Id
		/// </summary>
		/// <typeparam name="TAggregate"></typeparam>
		/// <param name="id">AggregateId of Aggregate who should restore</param>
		/// <returns></returns>
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