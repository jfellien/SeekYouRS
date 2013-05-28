using System;
using SeekYouRS.Tests.TestObjects.Events;

namespace SeekYouRS.Tests.TestObjects
{
    internal class Fahrzeug : Aggregate
    {
        public void Create(Guid id, string typ)
        {
            ApplyChanges(new FahrzeugWurdeErfasst{Id = id, Typ = typ});
        }

        internal string Typ
        {
            get { return FromHistory<FahrzeugWurdeErfasst>().Typ; }
        }

        public override Guid Id
        {
            get { return FromHistory<FahrzeugWurdeErfasst>().Id; }
        }
    }
}