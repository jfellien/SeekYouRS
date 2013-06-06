using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Events;

namespace SeekYouRS.Tests
{
    [TestFixture]
    internal class AggregateTests
    {
        [Test]
        public void TestToCreateCustomerViaAggregate()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var id = Guid.NewGuid();
            var customer = aggregateStore.GetAggregate<Customer>(id);

            customer.Create(id, "My Customer");
            aggregateStore.Save(customer);

            customer.Name.Should().BeEquivalentTo("My Customer");
        }

        [Test]
        public void TestToCreateCustomerAndVehicle()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var customerId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();

            var customer = aggregateStore.GetAggregate<Customer>(customerId);
            var vehicle = aggregateStore.GetAggregate<Vehicle>(vehicleId);

            customer.Create(customerId, "My Customer");
            aggregateStore.Save(customer);

            customer.Id.ShouldBeEquivalentTo(customerId);
            customer.Name.Should().BeEquivalentTo("My Customer");
            aggregateStore.GetAggregate<Customer>(customerId)
                .History.OfType<AggregateEventBag<CustomerCreated>>().Any()
                .Should().BeTrue();

            vehicle.Create(vehicleId, "My Vehicle");
            aggregateStore.Save(vehicle);

            vehicle.Typ.ShouldBeEquivalentTo("My Vehicle");

            aggregateStore.GetAggregate<Vehicle>(vehicleId)
                .History.OfType<AggregateEventBag<VehicleCreated>>().Any()
                .Should().BeTrue();
        }

        [Test]
        public void TestToChangeCustomerName()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var customerId = Guid.NewGuid();

            var customer = aggregateStore.GetAggregate<Customer>(customerId);
            customer.Create(customerId, "My Customer");
            aggregateStore.Save(customer);

            customer.Name.Should().BeEquivalentTo("My Customer");
            customer.Id.ShouldBeEquivalentTo(customerId);

            customer.Change("New Name");
            customer.Name.Should().BeEquivalentTo("New Name");
            aggregateStore.Save(customer);

            customer.Changes.Count().ShouldBeEquivalentTo(0);
            customer.History.Count().ShouldBeEquivalentTo(2);
        }

        [Test]
        public void TestWithoutCreateAggregate()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var customerId = Guid.NewGuid();

            var customer = aggregateStore.GetAggregate<Customer>(customerId);

            aggregateStore.Save(customer);
            var expectedId = Guid.Empty;

            Assert.Catch<NullReferenceException>(() => expectedId = customer.Id);

        }
    }
}