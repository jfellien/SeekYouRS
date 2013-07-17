using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.Tests.TestObjects.Events
{
    public class CustomerCreated : IAmAnAggregateEvent
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}