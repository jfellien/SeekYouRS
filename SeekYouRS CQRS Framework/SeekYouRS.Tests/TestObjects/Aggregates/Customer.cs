using System;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Utilities;

namespace SeekYouRS.Tests.TestObjects.Aggregates
{
    internal class CustomerModel
    {
        private readonly string _name;
        private readonly Guid _id;

        public CustomerModel(Guid id, string name)
        {
            _name = name;
            _id = id;
        }

        public Guid Id { get { return _id; } }
        public string Name {get { return _name; }}

        public CustomerModel ChangeName(string newName)
        {
            return new CustomerModel(Id, newName);
        }
    }

    internal class Customer : GenericAggregate<CustomerModel>
    {
        public Customer()
        {
            base.RegisterModelTransformer<CustomerCreated>((co, ev) => (new CustomerModel(ev.Id, ev.Name)).Some());
            base.RegisterModelTransformer<CustomerRemoved>((co, ev) => Option.None<CustomerModel>());
            base.RegisterModelTransformer<CustomerChanged>((co, ev) => co.Map(c => c.ChangeName(ev.Name)));
        }

        public string Name
        {
            get
            {
                return
                base.AggregateCurrentModel()
                    .Map(m => m.Name)
                    .DefaultsTo(null);
            }
        }

        public override Guid Id {
            get
            {
                return
                base.AggregateCurrentModel()
                    .Map(m => m.Id)
                    .ThrowsOnNone(new NullReferenceException());
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

	    public void RaiseUnhandledEvent()
	    {
		    ApplyChanges(new UnhandlesEventRaised());
	    }
    }
}