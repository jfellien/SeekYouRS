using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.EventSource;

namespace SeekYouRS.Tests
{
    [TestFixture]
    class EventStoreTests
    {
        [Test]
        public void TestToSaveEvents()
        {
            var eventStore = new InMemoryEventStore();
            var eventId = Guid.NewGuid();

            var events = new List<Event>
                {
                    new EventBag<TestEvent>(eventId)
                        {
                            EventData = new TestEvent{Text = "HalloWelt"}
                        }
                };
            eventStore.Store(events);

            var savedEvents = eventStore.GetAggregateHistoryBy(eventId).ToList();

            savedEvents.Count().Should().Be(1);
            var savedEvent = savedEvents.FirstOrDefault(@event => @event.Id == eventId);
            savedEvent.Should().BeOfType<EventBag<TestEvent>>();
            savedEvent.As<EventBag<TestEvent>>().EventData.Text.Should().BeEquivalentTo("HalloWelt");
        }
    }

    [TestFixture]
    internal class AggregateTests
    {
        [Test]
        public void TestToCreateKundeViaAggregate()
        {
            var eventStore = new InMemoryEventStore();
            var id = Guid.NewGuid();
            var kunde = new Kunde{History = eventStore.GetAggregateHistoryBy(id)};

            kunde.Create(id, "Mein Kunde");
            eventStore.Store(kunde.Changes);

            kunde.Name.Should().BeEquivalentTo("Mein Kunde");
        }
    }

    [TestFixture]
    internal class KundeApiTests
    {
        [Test]
        public void TestToCreateKundeViaApi()
        {
            var eventStore = new InMemoryEventStore();
            var reciever = new KundenCommandReciever(eventStore);
            var queries = new KundenQueries();

            var api = new KundenApi(reciever,queries);
            var id = Guid.NewGuid();

            api.Commands.Recieve(new ErfasseKunde{Id = id, Name = "My Kunde"});

            var aggregateEvents = eventStore.GetAggregateHistoryBy(id).ToList();

            aggregateEvents.Count().ShouldBeEquivalentTo(1);
            aggregateEvents.OfType<EventBag<KundeWurdeErfasst>>().Should().NotBeNull();
            aggregateEvents.OfType<EventBag<KundeWurdeErfasst>>().LastOrDefault().EventData.Name.ShouldBeEquivalentTo("My Kunde");
        }
    }
}
