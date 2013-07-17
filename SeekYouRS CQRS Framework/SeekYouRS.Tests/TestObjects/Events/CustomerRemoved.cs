using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.Tests.TestObjects.Events
{
	internal class CustomerRemoved : IAmAnAggregateEvent
	{
		public Guid Id { get; set; }
	}
}