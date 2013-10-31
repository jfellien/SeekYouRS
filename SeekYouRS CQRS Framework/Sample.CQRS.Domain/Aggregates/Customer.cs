using System;
using Sample.CQRS.Contracts.Events;
using SeekYouRS.BaseComponents;

namespace Sample.CQRS.Domain.Aggregates
{
    internal class Customer : Aggregate
    {
        public string Name
        {
            get
            {
                var lastChange = FromHistory<CustomerChanged>();

                return lastChange != null
                           ? lastChange.Name
                           : FromHistory<CustomerCreated>().Name;
            }
        }

        public override Guid Id
        {
            get { return FromHistory<CustomerCreated>().Id; }
        }

        public void Create(Guid id, string name)
        {
            ApplyChanges(new CustomerCreated
            {
                Id = id,
                Name = name
            });
        }

        public void Change(string neuerName)
        {
            ApplyChanges(new CustomerChanged
            {
                Id = Id,
                Name = neuerName
            });
        }
    }
}