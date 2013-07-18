using System.Collections.Generic;
using System.Linq;
using SeekYouRS.BaseComponents;
using SeekYouRS.Contracts;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Queries
{
	public class CustomerQueries : QueryHandler
	{
		CustomerModel ExecuteQuery(GetCustomer query)
		{
			var customer = ReadModelStore.Retrieve<CustomerModel>()
			                             .SingleOrDefault(cust => cust.Id == query.Id);

			return customer;
		}

		IEnumerable<CustomerModel> ExecuteQuery(GetAllCustomers query)
		{
			return ReadModelStore.Retrieve<CustomerModel>();
		}
	}
}