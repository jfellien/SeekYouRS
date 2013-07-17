using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.Tests.TestObjects.Events
{
	internal class CustomerChanged : IAmAnAggregateEvent
	{
		public string Name { get; set; }

		public Guid Id { get; set; }
	}
}