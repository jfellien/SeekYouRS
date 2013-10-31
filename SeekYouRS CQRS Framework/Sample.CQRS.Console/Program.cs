using System;
using Sample.CQRS.Contracts.Commands;
using Sample.CQRS.Contracts.Models;
using Sample.CQRS.Contracts.Queries;
using Sample.CQRS.Domain.Context;
using Sample.CQRS.Infrastructure;
using SeekYouRS.EventStore;

namespace Sample.CQRS.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
            var readModelStore = new InMemoryReadModelStore();

            var api = new CustomerContext(eventRecorder, readModelStore);
            var id = Guid.NewGuid();

            api.Process(new CreateCustomer
            {
                Id = id,
                Name = "My Customer"
            });

            var customerModel = api.ExecuteQuery<CustomerModel>(new GetCustomer
            {
                Id = id
            });

            System.Console.WriteLine(customerModel.Name);
            System.Console.ReadLine();
        }
    }
}