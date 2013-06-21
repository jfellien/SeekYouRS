using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Store;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Queries
{
	public class CustomerQueries : Store.Queries
	{
		public CustomerQueries(IStoreReadModels readModelStore) : base(readModelStore)
		{
		}

		public override T Retrieve<T>(dynamic query)
		{ 
			return ExecuteQuery(query);
		}

		CustomerModel ExecuteQuery(GetCustomer query)
		{
			var customer = ReadModelStore.Query<CustomerModel>()
			                             .SingleOrDefault(cust => cust.Id == query.Id);

			return customer;
		}

		IEnumerable<CustomerModel> ExecuteQuery(GetAllCustomers query)
		{
			return ReadModelStore.Query<CustomerModel>();
		}
	}
}