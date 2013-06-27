using System;
using SeekYouRS.Contracts;

namespace SeekYouRS
{
	/// <summary>
	/// Base class for Query Repositories.
	/// </summary>
	public abstract class Queries : IQueryReadModels
	{
		protected Queries(IStoreAndRetrieveReadModels readModelStore)
		{
			ReadModelStore = readModelStore;
		}

		/// <summary>
		/// Gets the ReadModelStore who knows the ReadModels to retrieve
		/// </summary>
		public IStoreAndRetrieveReadModels ReadModelStore { get; set; }

		/// <summary>
		/// Derived method from interface IQueryReadModels.
		/// You should implement this method onlxy with call 'ExecuteQuery(query)' and
		/// implement for any Query an private ExecuteQuery method.
		/// </summary>
		/// <typeparam name="T">Type of expected returned ReadModel</typeparam>
		/// <param name="query">The Query to get ReadModel</param>
		/// <returns></returns>
		public abstract T Retrieve<T>(dynamic query);

		public object ExecuteQuery(object query)
		{
			throw new ArgumentException(String.Format("I'm so sorry, this query {0} is not assigned to this Queries Repository", query.GetType().Name));
		}
	}
}