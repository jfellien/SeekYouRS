using System.Collections.Generic;
using System.Linq;

namespace SeekYouRS.Store
{
	public interface IStoreReadModels
	{
		void Add(dynamic model);
		void Change(dynamic model);
		void Remove(dynamic model);
		IEnumerable<T> Query<T>();
	}
}