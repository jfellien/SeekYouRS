using System;

namespace SeekYouRS.Storing
{
    public interface IPublishAggregateEvents
    {
        event Action<AggregateEvent> Publish;
    }
}