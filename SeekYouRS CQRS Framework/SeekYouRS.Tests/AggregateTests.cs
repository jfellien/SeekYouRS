using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.EventStore;
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
	        var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var aggregates = new AggregateStore(eventRecorder);
			
            var id = Guid.NewGuid();
            var customer = aggregates.GetAggregate<Customer>(id);

            customer.Create(id, "My Customer");
            aggregates.Save(customer);

            customer.Name.Should().BeEquivalentTo("My Customer");
        }

        [Test]
        public void TestToCreateCustomerAndVehicle()
        {
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var aggregates = new AggregateStore(eventRecorder);

            var customerId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();

            var customer = aggregates.GetAggregate<Customer>(customerId);
            var vehicle = aggregates.GetAggregate<Vehicle>(vehicleId);

            customer.Create(customerId, "My Customer");
            aggregates.Save(customer);

            customer.Id.ShouldBeEquivalentTo(customerId);
            customer.Name.Should().BeEquivalentTo("My Customer");
            aggregates.GetAggregate<Customer>(customerId)
                .History.OfType<CustomerCreated>().Any()
                .Should().BeTrue();

            vehicle.Create(vehicleId, "My Vehicle");
            aggregates.Save(vehicle);

            vehicle.Typ.ShouldBeEquivalentTo("My Vehicle");

            aggregates.GetAggregate<Vehicle>(vehicleId)
                .History.OfType<VehicleCreated>().Any()
                .Should().BeTrue();
        }

        [Test]
        public void TestToChangeCustomerName()
        {
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var aggregates = new AggregateStore(eventRecorder);

            var customerId = Guid.NewGuid();

            var customer = aggregates.GetAggregate<Customer>(customerId);
            customer.Create(customerId, "My Customer");
            aggregates.Save(customer);

            customer.Name.Should().BeEquivalentTo("My Customer");

            customer.Change("New Name");
            customer.Name.Should().BeEquivalentTo("New Name");
            aggregates.Save(customer);

            customer.Changes.Count().ShouldBeEquivalentTo(0);
            customer.History.Count().ShouldBeEquivalentTo(2);

        }

        [Test]
        public void TestWithoutCreateAggregate()
        {
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var aggregates = new AggregateStore(eventRecorder);

            var customerId = Guid.NewGuid();

            var customer = aggregates.GetAggregate<Customer>(customerId);

            aggregates.Save(customer);
            var expectedId = Guid.Empty;

            Assert.Catch<NullReferenceException>(() => expectedId = customer.Id);

        }
    }
}