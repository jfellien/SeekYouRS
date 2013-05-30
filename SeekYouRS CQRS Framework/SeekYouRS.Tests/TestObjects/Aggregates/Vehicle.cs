using System;
using SeekYouRS.Tests.TestObjects.Events;

namespace SeekYouRS.Tests.TestObjects.Aggregates
{
    internal class Vehicle : Aggregate
    {
        public void Create(Guid id, string typ)
        {
            ApplyChanges(new VehicleCreated{Id = id, Typ = typ});
        }

        internal string Typ
        {
            get { return FromHistory<VehicleCreated>().Typ; }
        }

        public override Guid Id
        {
            get { return FromHistory<VehicleCreated>().Id; }
        }
    }
}