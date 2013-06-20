namespace SeekYouRS.Handler
{
	/// <summary>
	/// Represents an Event Handler fpr Aggregate Events.
	/// </summary>
	public interface IHandleAggregateEvents
	{
		/// <summary>
		/// Handles the AggregateEvent by saving the changes, decribed as properties of AggregateEvent. 
		/// </summary>
		/// <param name="aggregateEvent"></param>
		void SaveChangesBy(AggregateEvent aggregateEvent);
	}
}