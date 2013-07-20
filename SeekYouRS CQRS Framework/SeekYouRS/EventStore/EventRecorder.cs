using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SeekYouRS.Contracts;

namespace SeekYouRS.EventStore
{
	/// <summary>
	/// This component saves and retrieves AggregateEvents like a Recorder. 
	/// It is possible to get the recorded Events by calling the ReplayFor method
	/// </summary>
	public class EventRecorder
	{
		/// <summary>
		/// Concrete implementation of store
		/// </summary>
		readonly IStoreAndRetrieveAggregateEvents _unitOfWork;

		/// <summary>
		/// Raises if an Event is recorded
		/// </summary>
		public event Action<IAmAnAggregateEvent> EventHasStored;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="unitOfWork">Conrete implementation of Store</param>
		public EventRecorder(IStoreAndRetrieveAggregateEvents unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Puts the Event into Storage
		/// </summary>
		/// <param name="aggregateEvent"></param>
		public void Record(IAmAnAggregateEvent aggregateEvent)
		{
			_unitOfWork.Store(new AggregateEventBag
				{
					AggregateId = aggregateEvent.Id,
					TimeStamp = DateTime.Now,
					EventData = aggregateEvent
				});

			try
			{
				EventHasStored(aggregateEvent);
			}
			catch (NullReferenceException)
			{
				Trace.WriteLine("Nobody likes to hear me :´(");
			}
		}
		/// <summary>
		/// Reads the Events from Store
		/// </summary>
		/// <param name="aggregateId">Id of Aggregate who should read the Events</param>
		/// <returns></returns>
		public IEnumerable<IAmAnAggregateEvent> ReplayFor(Guid aggregateId)
		{
			return _unitOfWork.RetrieveBy(aggregateId)
				.OrderBy(aggregateEventBag => aggregateEventBag.TimeStamp)
			    .Select(aggregateEventBag => aggregateEventBag.EventData);
		}
	}
}
