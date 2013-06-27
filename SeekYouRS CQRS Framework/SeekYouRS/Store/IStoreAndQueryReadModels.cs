using System.Collections.Generic;

namespace SeekYouRS.Store
{
	/// <summary>
	/// Represents the Behaviour of a ReadModelStore
	/// </summary>
	public interface IStoreAndQueryReadModels
	{
		/// <summary>
		/// Adds a ReadModel to Store
		/// </summary>
		/// <param name="model"></param>
		void Add(dynamic model);
		/// <summary>
		/// Change a ReadModel from Store
		/// </summary>
		/// <param name="model"></param>
		void Change(dynamic model);
		/// <summary>
		/// Removes a ReadModel from Store
		/// </summary>
		/// <param name="model"></param>
		void Remove(dynamic model);
		/// <summary>
		/// Returns the list of T 
		/// </summary>
		/// <typeparam name="T">Type of ReadModel</typeparam>
		/// <returns></returns>
		IEnumerable<T> Query<T>();
	}
}