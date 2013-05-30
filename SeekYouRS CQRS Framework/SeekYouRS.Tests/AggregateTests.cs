using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.Storing;
using SeekYouRS.Tests.TestObjects;
using SeekYouRS.Tests.TestObjects.Aggregates;
using SeekYouRS.Tests.TestObjects.Events;

namespace SeekYouRS.Tests
{
    [TestFixture]
    internal class AggregateTests
    {
        [Test]
        public void TestToCreateKundeViaAggregate()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var id = Guid.NewGuid();
            var kunde = aggregateStore.GetAggregate<Customer>(id);

            kunde.Create(id, "Mein Customer");
            aggregateStore.Save(kunde);

            kunde.Name.Should().BeEquivalentTo("Mein Customer");
        }

        [Test]
        public void TestToCreateKundeAndFahrzeug()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var kundeId = Guid.NewGuid();
            var fahrzeugId = Guid.NewGuid();

            var kunde = aggregateStore.GetAggregate<Customer>(kundeId);
            var fahrzeug = aggregateStore.GetAggregate<Vehicle>(fahrzeugId);

            kunde.Create(kundeId, "Mein Customer");
            aggregateStore.Save(kunde);

            kunde.Id.ShouldBeEquivalentTo(kundeId);
            kunde.Name.Should().BeEquivalentTo("Mein Customer");
            aggregateStore.GetAggregate<Customer>(kundeId)
                .History.OfType<AggregateEventBag<CustomerCreated>>().Any()
                .Should().BeTrue();

            fahrzeug.Create(fahrzeugId, "Mein Vehicle");
            aggregateStore.Save(fahrzeug);

            fahrzeug.Typ.ShouldBeEquivalentTo("Mein Vehicle");

            aggregateStore.GetAggregate<Vehicle>(fahrzeugId)
                .History.OfType<AggregateEventBag<VehicleCreated>>().Any()
                .Should().BeTrue();
        }

        [Test]
        public void TestToChangeKundenname()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var kundeId = Guid.NewGuid();

            var kunde = aggregateStore.GetAggregate<Customer>(kundeId);
            kunde.Create(kundeId, "Mein Customer");
            aggregateStore.Save(kunde);

            kunde.Name.Should().BeEquivalentTo("Mein Customer");

            kunde.Change("Neuer Name");
            kunde.Name.Should().BeEquivalentTo("Neuer Name");
            aggregateStore.Save(kunde);

            kunde.Changes.Count().ShouldBeEquivalentTo(0);
            kunde.History.Count().ShouldBeEquivalentTo(2);

        }
    }
}