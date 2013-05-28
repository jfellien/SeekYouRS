using System;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.Tests.TestObjects;
using SeekYouRS.Tests.TestObjects.Commands;
using SeekYouRS.Tests.TestObjects.Queries;

namespace SeekYouRS.Tests
{
    [TestFixture]
    internal class KundeApiTests
    {
        [Test]
        public void TestToCreateKundeViaApi()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var queriesStore = new InMemoryQueriesStore();

            var commands = new CustomerCommandsHandler(aggregateStore);
            var queries = new CustomerQueriesHandler(queriesStore);

            var api = new CustomerApi(commands, queries);
            var id = Guid.NewGuid();

            api.Process(new ErfasseKunde{Id = id, Name = "My Customer"});

            var kunde = aggregateStore.GetAggregate<Customer>(id);

            kunde.Name.ShouldBeEquivalentTo("My Customer");
        }

        [Test]
        public void TestToCreateAndChangeKundeViaApi()
        {
            var aggregateStore = new InMemoryAggregateStore();
            var queriesStore = new InMemoryQueriesStore();

            var reciever = new CustomerCommandsHandler(aggregateStore);
            var queries = new CustomerQueriesHandler(queriesStore);

            var api = new CustomerApi(reciever, queries);
            var id = Guid.NewGuid();

            api.Process(new ErfasseKunde { Id = id, Name = "My Customer" });
            api.Process(new ÄndereKunde { Id = id, Name = "Neuer Name" });

            var kunde = aggregateStore.GetAggregate<Customer>(id);

            kunde.Name.ShouldBeEquivalentTo("Neuer Name");
        }
    }
}