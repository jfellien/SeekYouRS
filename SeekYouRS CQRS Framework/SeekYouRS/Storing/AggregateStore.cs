namespace SeekYouRS.Storing
{
    public interface AggregateStore : IStoreAggregateEvents, IPublishAggregateEvents {}
}