namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Describes an EventHandler for AggregateEvents.
	/// </summary>
	public interface IHandleAggregateEvents
	{
		/// <summary>
		/// Handles the AggregateEvent by saving the changes. 
		/// </summary>
		/// <param name="aggregateEvent"></param>
		void Handle(IAmAnAggregateEvent aggregateEvent);

		/// <summary>
		/// Gets or sets the concrete ReadModelStore who saves and retrieves the Readmodels resulted from the AggregateEvents
		/// </summary>
		IStoreAndRetrieveReadModels ReadModelStore { get; set; }
	}
}