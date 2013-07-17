using System;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.Tests.TestObjects
{
	public class TestEvent : IAmAnAggregateEvent
	{
		public Guid Id { get; set; }

		public string Wert { get; set; }
	}
}