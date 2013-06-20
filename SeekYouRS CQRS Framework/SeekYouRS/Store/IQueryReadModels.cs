namespace SeekYouRS.Store
{
	/// <summary>
	/// Represents the behaviour of a component who can returns query results
	/// </summary>
	public interface IQueryReadModels
	{
		/// <summary>
		/// ececutes the query and returns the result
		/// </summary>
		/// <typeparam name="T">Type of expected return value</typeparam>
		/// <param name="query">The Query</param>
		/// <returns>Result of Query</returns>
		T Retrieve<T>(dynamic query);
	}
}