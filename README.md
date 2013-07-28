[TOC]

# SeekYouRS


Public Open Source CQRS Framework - very simple, developed by KISS principle


## Usage

### Installation

Load the package by Nuget using the Package Manager Console

	install-package SeekYouRS

### Implementation

Your CQRS solution needs follow main components:


1.  Domain Context
1.  Command Handler
1.  Event Handler
1.  Query Handler
1.  Aggregate
1.  Commands and Events

#### Domain Context ####

This component binds all working handler and enabled the connection between Domain and EventStore. The bindable components are  CommandHandler, QueryHandler and EventHandler.

This is a artificaly kind to describe context of domain driven design.
 
Implemet this component as first in your project. So you can see what is the next important step for implementation.
	
	// Example:
	public class CustomerContext : DomainContext<CustomerCommandHandler, CustomerQueriesHandler, CustomerEventHandler>
	{
		public CustomerContext(EventRecorder eventRecorder, IStoreAndRetrieveReadModels readModelStore) 
			: base(eventRecorder, readModelHandler) { }
	}

Ready... This is the fastes way to build a context. But you should create for any Aggregate a Context.


### Command Handler ###

This component handles commands (what for a surprise ;-)). Commands are data objects who instructs a change of domain state.
Implement a Command Handler by derive from `CommandHandler`, like this:

	// Example:
	public class CustomerCommands : CommandHandler
	{
		// It is very important to implement this follow lines of code.
		// Begin
	  	public override void Handle(dynamic command)
		{
			HandleThis(command);
		}

		public override TResult Handle<TResult>(dynamic command)
		{
			return HandleThis(command);
		}
		// End
		
		// Every Command needs its own prive HandleThis method
		// Here a sample to create a new customer
		void HandleThis(CreateCustomer command)
		{
			var customer = new Customer();
			customer.Create(command.Id, command.Name);
			
			// Save the new state
			AggregateStore.Save(customer);
		}
		
		// After the creation of Customer Aggreagte, 
		// you should read the Aggregate form AggregateStore. 
		// Is restores the last state of your Aggreagte
		void HandleThis(RemoveCustomer command)
		{
			// Restore the customer from AggregateStore
			var customer = AggregateStore.GetAggregate<Customer>(command.Id);
			customer.Remove();
			
			// Save the new state
			AggregateStore.Save(customer);
		}
	}
	
OK, the Command Handler is ready to use... **Note, any Command needs its own private `HandleThis` method**


### Event Handler ###

After the execution of a Command the state changes will be published. One of the subscriber is the ReadModelStore, where saved the actual state of vor Domain.
Implement a Event Handler by derive from `EventHandler`, like this:

	// Example:
	public class CustomerEventHandler : EventHandler
	{
		// It is very important to implement this follow lines of code
		// Begin
		public override void Handle(IAmAnAggregateEvent aggregateEvent)
		{
			HandleThis((dynamic)aggregateEvent);
		}
		// End
		
		// Every Event needs its own prive HandleThis method
		// Here a sample after creating new customer
		void HandleThis(CustomerCreated customerCreated)
		{
			ReadModelStore.Add(new CustomerModel
				{
					Id = customerCreated.Id,
					Name = customerCreated.Name
				});
		}
	}

OK, the Event Handler is ready to use... **Note, any Event needs its own private `HandleThis` method**
	
#### Query Handler ####

This component ist not realy a Handler. It is more a router for queries and its results. But the behaviour of this component feels like a handler.

Implement a Query Handler by derive from `QueryHandler`, like this:

	// Example:
	public class CustomerQueryHandler : QueryHandler
	{
		// It is very important to implement this follow lines of code
		// Begin
		public override T Retrieve<T>(dynamic query)
		{ 
			return ExecuteQuery(query);
		}
		// End
		
		// Every Query needs its own ExecuteQuery method		// Here a sample to get a customer by Id
		CustomerModel ExecuteQuery(GetCustomer query)
		{
			var customer = ReadModelStore.Retrieve<CustomerModel>()
										 .SingleOrDefault(cust => cust.Id == query.Id);

			return customer;
	
		// This method get a list of all customers
		IEnumerable<CustomerModel> ExecuteQuery(GetAllCustomers query)
		{
			return ReadModelStore.Retrieve<CustomerModel>();
		}
	}

OK, the Query Handler is ready to use... **Note, any Query needs its own private `ExecuteQuery` method**

#### Aggregate ####

Aggregates bounds business logic an state of your business. Manage all your domain behaviour in an Aggregate but never ever infrastructure. Aggregates never publish its state by properties, only the Id can be public.

Use the internal `FromHistory` method to get the state of your Aggregate instead of fields. `FromHistory`contains all state changes, so it is easier to read state. 

Here a simple Aggregate:

	// Example:
	internal class Customer : Aggregate
	{
		string Name
		{
		  get
		  {
			// Use the FromHistory Method to get the lastes known data
			var removed = FromHistory<CustomerRemoved>();
			if (removed != null)
			  return null;
			
			// Use the FromHistory Method to get the lastes known data
			var lastChange = FromHistory<CustomerChanged>();
			
			return lastChange != null 
			  ? lastChange.Name 
			  : FromHistory<CustomerCreated>().Name;
		  }
		}
		
		public override Guid Id 
		{
		  get
		  {
			// Use the FromHistory Method to get the lastes known data
			var removed = FromHistory<CustomerRemoved>();
			return removed != null ? Guid.Empty : FromHistory<CustomerCreated>().Id;
		  }
		}
		
		public void Create(Guid id, string name)
		{
		  // Everytime use the ApplyChanges Method to save the new state of Aggregate
		  ApplyChanges(new CustomerCreated { Id = id, Name = name });
		}
		
		public void Change(string neuerName)
		{
		  // Everytime use the ApplyChanges Method to save the new state of Aggregate
		  ApplyChanges(new CustomerChanged { Name = neuerName });
		}
		
		public void Remove()
		{
		  // Everytime use the ApplyChanges Method to save the new state of Aggregate
		  ApplyChanges(new CustomerRemoved());
		}
	}
	
To publish your state changes of the Aggregate you should use the `ApplyChanges` method with a special AggreagteEvent object, that contains all state change data. This AggreagteEvent will send to Event Handler after saving the Aggreagte in Command Handler.

#### Commands and Events ####

You need Commands to change the state of your Aggregates. Feel free to formulate the Commands. You nedd no interface or base class. But the Aggregate Events should implements the interface `IAmAnAggregateEvent`. This is the best way to ensure any event object has an Id.

	//Example:
	public class CustomerCreated : IAmAnAggregateEvent
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }