namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Represents an Event Handler for Aggregate Events.
	/// </summary>
	public interface IHandleAggregateEvents
	{
		/// <summary>
		/// Handles the AggregateEvent by saving the changes, decribed as properties of AggregateEvent. 
		/// </summary>
		/// <param name="aggregateEvent"></param>
		void Handle(IAmAnAggregateEvent aggregateEvent);

		IStoreAndRetrieveReadModels ReadModelStore { get; set; }
	}
}