using SeekYouRS.Storing;

namespace SeekYouRS.Messaging
{
    public interface IRetrieveModels
    {
        T Execute<T>(dynamic query);
        void HandleChanges(AggregateEvent aggregateEvent);
    }
}
