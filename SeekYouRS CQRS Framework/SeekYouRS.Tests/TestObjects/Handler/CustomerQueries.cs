using System;
using SeekYouRS.Messaging;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Models;

namespace SeekYouRS.Tests.TestObjects.Handler
{
    public class CustomerQueries : IRetrieveModels
    {
        private readonly IStoreAggregateEventsAsModels _readModel;

        public CustomerQueries(IStoreAggregateEventsAsModels readModel)
        {
            _readModel = readModel;
        }

        public T Execute<T>(dynamic query)
        {
            return _readModel.Retrieve<T>(query);
        }

        public IStoreAggregateEventsAsModels ModelStore {
            get { return _readModel; }
        }
    }
}