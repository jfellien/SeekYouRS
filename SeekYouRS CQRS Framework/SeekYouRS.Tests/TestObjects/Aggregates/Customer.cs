using System;
using SeekYouRS.Tests.TestObjects.Events;

namespace SeekYouRS.Tests.TestObjects.Aggregates
{
    internal class Customer : Aggregate
    {
        public string Name
        {
            get
            {
                var removed = FromHistory<CustomerRemoved>();
                if (removed != null)
                    return null;

                var lastChange = FromHistory<CustomerChanged>();
                
                return lastChange != null 
                    ? lastChange.Name 
                    : FromHistory<CustomerCreated>().Name;
            }
        }

        public override Guid Id {
            get
            {
                var removed = FromHistory<CustomerRemoved>();
                return removed != null ? Guid.Empty : FromHistory<CustomerCreated>().Id;
            }
        }

        public void Create(Guid id, string name)
        {
            ApplyChanges(new CustomerCreated { Id = id, Name = name });
        }

        public void Change(string neuerName)
        {
            ApplyChanges(new CustomerChanged { Name = neuerName });
        }

        public void Remove()
        {
            ApplyChanges(new CustomerRemoved());
        }
    }
}