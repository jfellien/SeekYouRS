using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Contracts;

namespace Sample.CQRS.Infrastructure
{
	public class InMemoryReadModelStore : IStoreAndRetrieveReadModels
	{
		private readonly List<dynamic> _data;

		public InMemoryReadModelStore()
		{
			_data = new List<dynamic>();
		}

		public void Add(dynamic model)
		{
			_data.Add(model);
		}

		public void Change(dynamic model)
		{
			var oldObject = _data.SingleOrDefault(o => o.Id == model.Id);
			_data.Remove(oldObject);
			_data.Add(model);
		}

		public void Remove(dynamic model)
		{
			var oldObject = _data.SingleOrDefault(o => o.Id == model.Id);
			_data.Remove(oldObject);
		}

		public IEnumerable<T> Retrieve<T>()
		{
			return _data.OfType<T>().AsEnumerable();
		}
	}
}