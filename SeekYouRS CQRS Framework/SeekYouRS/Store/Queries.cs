using System;

namespace SeekYouRS.Store
{
	public abstract class Queries : IQueryReadModels
	{
		protected Queries(IStoreReadModels readModelStore)
		{
			ReadModelStore = readModelStore;
		}

		public IStoreReadModels ReadModelStore { get; set; }

		public abstract T Retrieve<T>(dynamic query);

		internal void ExecuteQuery(object query)
		{
			throw new ArgumentException(String.Format("I'm so sorry, this query {0} is not assigned to this Queries Repository", query.GetType().Name));
		}
	}
}