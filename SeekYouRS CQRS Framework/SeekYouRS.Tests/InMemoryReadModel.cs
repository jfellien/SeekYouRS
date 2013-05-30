using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects.Models;
using SeekYouRS.Tests.TestObjects.Queries;

namespace SeekYouRS.Tests
{
    internal class InMemoryReadModel : IStoreModels
    {
        private readonly List<dynamic> _data;

        public InMemoryReadModel()
        {
            _data = new List<object>();
        }

        public void Add(dynamic model)
        {
            _data.Add(model);
        }

        public void Change(dynamic model)
        {
            var oldObject =_data.SingleOrDefault(o => o.Id == model.Id);
            _data.Remove(oldObject);
            _data.Add(model);

        }

        public void Remove(dynamic model)
        {
            var oldObject = _data.SingleOrDefault(o => o.Id == model.Id);
            _data.Remove(oldObject);
        }

        public T Retrieve<T>(dynamic query)
        {
            return ExecuteQuery(query);
        }

        CustomerModel ExecuteQuery(GetCustomer query)
        {
            var customer = _data.OfType<CustomerModel>().SingleOrDefault(cust => cust.Id == query.Id);

            return customer;
        }
    }
}