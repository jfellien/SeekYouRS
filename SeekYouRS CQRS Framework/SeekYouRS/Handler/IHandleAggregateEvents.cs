namespace SeekYouRS.Handler
{
	public interface IHandleAggregateEvents
	{
		void SaveChangesBy(AggregateEvent aggregateEvent);
	}
}