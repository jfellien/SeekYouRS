using System;
using System.Linq;

namespace SeekYouRS.Tests
{
    internal class Kunde : Aggegate
    {
        public void Create(Guid id, string name)
        {
            ApplyChanges(new KundeWurdeErfasst(){Id = id, Name = name});
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