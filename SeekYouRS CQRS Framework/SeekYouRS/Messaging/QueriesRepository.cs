using SeekYouRS.Storing;

namespace SeekYouRS.Messaging
{
    public interface QueriesRepository
    {
        T Execute<T>(dynamic query);
        void HandleChanges(AggregateEvent aggregateEvent);
    }
}
