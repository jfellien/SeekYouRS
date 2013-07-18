namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Describes the behaviour of a component who can returns query results like an QueryHandler
	/// </summary>
	public interface IQueryReadModels
	{
		/// <summary>
		/// Executes the query and returns the result
		/// </summary>
		/// <typeparam name="T">Type of expected return value</typeparam>
		/// <param name="query">The Query</param>
		/// <returns>Result of Query</returns>
		T Retrieve<T>(dynamic query);

		/// <summary>
		/// Gets or sets the concrete ReadModelStore who saves and retrieves the ReadModels
		/// </summary>
		IStoreAndRetrieveReadModels ReadModelStore { get; set; }
	}
}