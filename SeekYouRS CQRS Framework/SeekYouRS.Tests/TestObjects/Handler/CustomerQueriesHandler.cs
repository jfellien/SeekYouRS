using System;
using SeekYouRS.Messaging;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
    public class CustomerQueriesHandler : IRetrieveModels
    {
        private readonly IStoreModels _readModel;

        public CustomerQueriesHandler(IStoreModels readModel)
        {
            _readModel = readModel;
        }

        public T Execute<T>(dynamic query)
        {
            return _readModel.Retrieve<T>(query);
        }

        public void HandleChanges(AggregateEvent aggregateEvent)
        {
            Handle((dynamic)aggregateEvent);
        }

        void Handle(object unassignedEvent)
        {
            var eventData = ((dynamic) unassignedEvent).EventData;

            throw new ArgumentException("This event is not assigned to this instance, " + eventData.GetType().Name);
        }

        void Handle(AggregateEventBag<CustomerCreated> customerCreated)
        {
            _readModel.Add(new CustomerModel
                {
                    Id = customerCreated.EventData.Id,
                    Name = customerCreated.EventData.Name
                });
        }

        void Handle(AggregateEventBag<CustomerChanged> customerChanged)
        {
            _readModel.Change(new CustomerModel
            {
                Id = customerChanged.Id,
                Name = customerChanged.EventData.Name
            });
        }

        void Handle(AggregateEventBag<CustomerRemoved> customerRemoved)
        {
            _readModel.Remove(new CustomerModel{Id = customerRemoved.Id});
        }
    }
}