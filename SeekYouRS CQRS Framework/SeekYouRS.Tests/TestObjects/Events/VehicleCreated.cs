using System;
using SeekYouRS.Contracts;

namespace SeekYouRS.Tests.TestObjects.Events
{
    internal class VehicleCreated : IAmAnAggregateEvent
    {
        public Guid Id { get; set; }

        public string Typ { get; set; }
    }
}