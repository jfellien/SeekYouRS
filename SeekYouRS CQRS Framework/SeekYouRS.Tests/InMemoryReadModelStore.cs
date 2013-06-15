using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Store;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;
using SeekYouRS.Tests.TestObjects.Queries;

namespace SeekYouRS.Tests
{
    internal class InMemoryReadModelStore : IStoreAggregateEventsAsReadModels
    {
        private readonly List<dynamic> _data;

        public InMemoryReadModelStore()
        {
            _data = new List<object>();
        }

        public void SaveChangesBy(AggregateEvent aggregateEvent)
        {
            Handle((dynamic)aggregateEvent);
        }

        void Handle(object unassignedEvent)
        {
            var eventData = ((dynamic)unassignedEvent).EventData;

            throw new ArgumentException("This event is not assigned to this instance, " + eventData.GetType().Name);
        }

        void Handle(AggregateEventBag<CustomerCreated> customerCreated)
        {
            Add(new CustomerModel
            {
                Id = customerCreated.EventData.Id,
                Name = customerCreated.EventData.Name
            });
        }

        void Handle(AggregateEventBag<CustomerChanged> customerChanged)
        {
            Change(new CustomerModel
            {
                Id = customerChanged.Id,
                Name = customerChanged.EventData.Name
            });
        }

        void Handle(AggregateEventBag<CustomerRemoved> customerRemoved)
        {
            Remove(new CustomerModel { Id = customerRemoved.Id });
        }

        void Add(dynamic model)
        {
            _data.Add(model);
        }

        void Change(dynamic model)
        {
            var oldObject = _data.SingleOrDefault(o => o.Id == model.Id);
            _data.Remove(oldObject);
            _data.Add(model);

        }

        void Remove(dynamic model)
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

        IEnumerable<CustomerModel> ExecuteQuery(GetAllCustomers query)
        {
            return _data.OfType<CustomerModel>();
        }
    }
}