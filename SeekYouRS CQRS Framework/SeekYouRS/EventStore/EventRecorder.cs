using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SeekYouRS.Contracts;

namespace SeekYouRS.EventStore
{
	public class EventRecorder
	{
		readonly IStoreAndRetrieveAggregateEvents _unitOfWork;

		public event Action<IAmAnAggregateEvent> EventHasStored;

		public EventRecorder(IStoreAndRetrieveAggregateEvents unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

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

		public IEnumerable<IAmAnAggregateEvent> ReplayFor(Guid aggregateId)
		{
			return _unitOfWork.RetrieveBy(aggregateId)
			            .Select(aggregateEventBag => aggregateEventBag.EventData);
		}
	}
}
