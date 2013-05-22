using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
