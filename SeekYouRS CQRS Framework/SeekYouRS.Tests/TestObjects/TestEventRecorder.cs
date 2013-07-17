using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.EventStore;

namespace SeekYouRS.Tests.TestObjects
{
	[TestFixture]
	public class TestEventRecorder
	{
		[Test]
		public void WriteEventsToRecorder()
		{
			var eventSource = new InMemoryAggregateEventStore();
			var sut = new EventRecorder(eventSource);

			var eventsCount = 0;

			sut.EventHasStored += @event => eventsCount++;

			sut.Record(new TestEvent() { Wert = "A" });
			sut.Record(new TestEvent() { Wert = "B" });
			sut.Record(new TestEvent() { Wert = "C" });
			sut.Record(new TestEvent() { Wert = "D" });
			sut.Record(new TestEvent() { Wert = "E" });
			sut.Record(new TestEvent() { Wert = "F" });

			eventsCount.ShouldBeEquivalentTo(6);
		}

		[Test]
		public void WriteAndGetEventsToRecorder()
		{
			var eventSource = new InMemoryAggregateEventStore();
			var sut = new EventRecorder(eventSource);
			var aggregateId = Guid.NewGuid();

			sut.EventHasStored += @event => Trace.WriteLine((object) @event.GetType());

			sut.Record(new TestEvent() { Id = aggregateId, Wert = "A" });
			sut.Record(new TestEvent() { Id = aggregateId, Wert = "B" });
			sut.Record(new TestEvent() { Id = aggregateId, Wert = "C" });
			sut.Record(new TestEvent() { Id = aggregateId, Wert = "D" });
			sut.Record(new TestEvent() { Id = aggregateId, Wert = "E" });
			sut.Record(new TestEvent() { Id = aggregateId, Wert = "F" });

			var events = sut.ReplayFor(aggregateId);

			AssertionExtensions.ShouldBeEquivalentTo<int>(events.Count(), 6);
		}
	}
}