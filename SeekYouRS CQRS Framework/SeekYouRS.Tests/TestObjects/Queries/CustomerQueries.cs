using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Store;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Queries
{
	public class CustomerQueries : IQueryReadModels
	{
		readonly IStoreReadModels _readModelStore;

		public CustomerQueries(IStoreReadModels readModelStore)
		{
			_readModelStore = readModelStore;
		}

		public T Retrieve<T>(dynamic query)
		{
			return ExecuteQuery(query);
		}

		CustomerModel ExecuteQuery(GetCustomer query)
		{
			var customer = _readModelStore.Query<CustomerModel>()
			                              .OfType<CustomerModel>()
			                              .SingleOrDefault(cust => cust.Id == query.Id);

			return customer;
		}

		IEnumerable<CustomerModel> ExecuteQuery(GetAllCustomers query)
		{
			return _readModelStore.Query<CustomerModel>().OfType<CustomerModel>();
		}
	}
}