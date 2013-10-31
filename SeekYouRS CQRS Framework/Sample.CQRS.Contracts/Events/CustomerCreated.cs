using System;
using SeekYouRS.Contracts;

namespace Sample.CQRS.Contracts.Events
{
    public class CustomerCreated : IAmAnAggregateEvent
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}