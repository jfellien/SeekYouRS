using System;
using SeekYouRS.Tests.TestObjects.Events;

namespace SeekYouRS.Tests.TestObjects
{
    internal class Customer : Aggregate
    {
        public string Name
        {
            get
            {
                var lastChange = FromHistory<KundeWurdeGe�ndert>();
                
                return lastChange != null 
                    ? lastChange.Name 
                    : FromHistory<KundeWurdeErfasst>().Name;
            }
        }

        public override Guid Id {
            get
            {
                return FromHistory<KundeWurdeErfasst>().Id; 
            }
        }

        public void Create(Guid id, string name)
        {
            // ID ist mit im AggregateEvent, weil ja das Aggregat ine ID braucht
            // andere AggregateEvent bekommen die ID nicht
            ApplyChanges(new KundeWurdeErfasst { Id = id, Name = name });
        }

        public void Change(string neuerName)
        {
            ApplyChanges(new KundeWurdeGe�ndert { Name = neuerName });
        }
    }
}