using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class for a QueryHandler.
	/// Any query should get a ExecuteQuery method in derived class.
	/// </summary>
	public abstract class QueryHandler : IQueryReadModels
	{
		/// <summary>
		/// Gets the ReadModelStore who knows the ReadModels to retrieve
		/// </summary>
		public IStoreAndRetrieveReadModels ReadModelStore { get; set; }

		/// <summary>
		/// Calls the concrete ExecuteQuery method of derived class.
		/// </summary>
		/// <typeparam name="T">Type of expected returned ReadModel</typeparam>
		/// <param name="query">The Query to get ReadModel</param>
		/// <returns></returns>
		public T Retrieve<T>(dynamic query)
		{
			return ExecuteQuery(query);
		}

		/// <summary>
		/// This is the fallback method to inform that a Query is not assigned.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public object ExecuteQuery(object query)
		{
			throw new ArgumentException("Unknown Query detected: " 
				+ query.GetType().Name
				+ " You should provide an ExecuteQuery method for this Query");
		}
	}
}