using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.Tests.TestObjects.Events
{
	internal class UnhandlesEventRaised : IAmAnAggregateEvent
	{
		public Guid Id { get; set; }
	}
}