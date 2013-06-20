using System;
using System.Linq;

namespace SeekYouRS.Store
{
	/// <summary>
	/// Sealed class, represents an Aggregates repository.
	/// </summary>
	public sealed class Aggregates
	{
		readonly IStoreAndRetrieveAggregates _unitOfWork;

		/// <summary>
		/// Raises if Aggregate has changed
		/// </summary>
		public event Action<AggregateEvent> AggregateHasChanged;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="unitOfWork">Concrete implementation of AggregateStore</param>
		public Aggregates(IStoreAndRetrieveAggregates unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Saves an Aggregate with the changes
		/// </summary>
		/// <typeparam name="TAggregate">Type of Aggregate</typeparam>
		/// <param name="aggregate">Instance of Aggregate</param>
		public void Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
		{
			_unitOfWork.Save(aggregate.Changes);

			if (AggregateHasChanged != null)
				foreach (var e in aggregate.Changes)
					AggregateHasChanged(e);

			aggregate.History = aggregate.History.Concat(aggregate.Changes).ToList();
			aggregate.Changes.Clear();
		}
		
		/// <summary>
		/// Gets an Aggregate
		/// </summary>
		/// <typeparam name="TAggregate">Type of Aggregate</typeparam>
		/// <param name="id">Id of Aggregate who will gets return</param>
		/// <returns>Instance of Aggregate</returns>
		public TAggregate GetAggregate<TAggregate>(Guid id) where TAggregate : Aggregate, new()
		{
			var aggregateHistory = _unitOfWork.GetEventsBy(id).ToList();
			var aggregate = new TAggregate
				{
					History = aggregateHistory
				};
			return aggregate;
		}
	}
}