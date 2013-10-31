using System;
using FluentAssertions;
using NUnit.Framework;
using Sample.CQRS.Contracts.Commands;
using Sample.CQRS.Contracts.Models;
using Sample.CQRS.Contracts.Queries;
using Sample.CQRS.Domain.Context;
using Sample.CQRS.Infrastructure;
using SeekYouRS.EventStore;

namespace Sample.CQRS.Tests
{
    [TestFixture]
    public class CustomerContextTests
    {
        [Test]
        public async void TestToChangeACustomer()
        {
            var eventRecorder = new EventRecorder(new RavenDBAggregateEventStore());
            var readModelStore = new RavenDBReadModelStore();

            var api = new CustomerContext(eventRecorder, readModelStore);
            var id = Guid.NewGuid();

            await api.Process(new CreateCustomer
            {
                Id = id,
                Name = "My Customer"
            });

            await api.Process(new ChangeCustomer
            {
                Id = id,
                Name = "New Name"
            });

            var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
            {
                Id = id
            });

            customer.Name.ShouldBeEquivalentTo("New Name");
        }

        [Test]
        public async void TestToCreateCustomerAndReadIt()
        {
            var eventRecorder = new EventRecorder(new RavenDBAggregateEventStore());
            var readModelStore = new RavenDBReadModelStore();

            var api = new CustomerContext(eventRecorder, readModelStore);
            var id = Guid.NewGuid();

            await api.Process(new CreateCustomer
            {
                Id = id,
                Name = "My Customer"
            });

            var customerModel = api.ExecuteQuery<CustomerModel>(new GetCustomer
            {
                Id = id
            });

            customerModel.Name.ShouldBeEquivalentTo("My Customer");
        }
    }
}