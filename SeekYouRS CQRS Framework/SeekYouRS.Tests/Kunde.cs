using System;

namespace SeekYouRS.Tests
{
    internal class Kunde : Aggegate
    {
        public void Create(Guid id, string name)
        {
            ApplyChanges(new KundeWurdeErfasst{Name = name});
        }

        internal string Name
        {
            get
            {
                return FromHistory<KundeWurdeErfasst>().Name; 
            }
        }
    }
}