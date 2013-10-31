using System.Linq;
using Sample.CQRS.Contracts.Models;
using Sample.CQRS.Contracts.Queries;

namespace Sample.CQRS.ReadModel.QueryHandler
{
    public class CustomerQueryHandler : SeekYouRS.BaseComponents.QueryHandler
    {
        public override T Retrieve<T>(dynamic query)
        {
            return ExecuteQuery(query);
        }

        CustomerModel ExecuteQuery(GetCustomer query)
        {
            var customer = ReadModelStore.Retrieve<CustomerModel>()
                                         .SingleOrDefault(cust => cust.Id == query.Id);

            return customer;
        }
    }
}