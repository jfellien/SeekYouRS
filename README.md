# SeekYouRS


Public Open Source CQRS Framework - very simple, developed by KISS principle


## Usage

### Installation

Load the package by Nuget using the Package Manager Console

    install-package SeekYouRS

### Implementation

Your CQRS solution needs follow main components:

1.  Aggregates
2.  Command Handler
3.  ReadModel Handler
4.  Context
5.  Aggregate
6.  Commands and Events

#### Aggregates ####

This component saves and returns your Aggregates from a specific data store. 
Use an unit of work to implements your data store. The unit of work should implements the interface `IAmAnAggregatesUnitOfWork`.

    // your implementation of unit of work
    public class MyAggregateStore : IAmAnAggregatesUnitOfWork
    {
      public void Save(IEnumerable<AggregateEvent> changes)
      {
        // your special save mechanism
      }
      
      public IEnumerable<AggregateEvent> GetEventsBy(Guid id)
      {
        // your high sophisticated get mechanism 
      }
    }
    
Use your implementation to inject the unit of work into the existing `Aggregates` component.

    var unitOfWork = new MyAggregateStore();
    var aggregates = new Aggregates(unitOfWork);
    
Ready, now you can get and save aggregates. To do this, you need a Command Handler.

### Command Handler ###

This component handles commands (what for a surprise ;-)). Commands are objects who instructs a change of domain state.
Implement a Command Handler by derive from `CommandHandler`, see:

    // Example:
    public class CustomerCommands : CommandHandler
    {
      public CustomerCommands(IStoreAggregates aggregateStore) : base(aggregateStore){}
      
      // very important, because the dynamic routes the command to the correct Execute implementation
      public override void Process(dynamic command)
      {
        Execute(command);
      }
      
      // Command executions should be private to mask the API
      private void Execute(CreateCustomer command)
      {
        // Hey dear... do you see the AggregateStore component. This is the component from above in action
        var customer = AggregateStore.GetAggregate<Customer>(command.Id);
        
        customer.Create(command.Id, command.Name);
        AggregateStore.Save(customer);
        }
    }
    
OK, the Command Handler is ready to use... **Note, any Command needs its own `Execute` method**


### ReadModel Handler ###

After the execution of a command the ReadModels needs an update of current state. The component ReadModel Handler recieve the changes by AggregateEvents.
To implements a ReadModel Handler you should implements the interface `IStoreAggregateEventsAsReadModels`.

Feel free to design your own ReadModel Handler. Here an example of my first testable component:

    internal class InMemoryReadModelStore : IStoreAggregateEventsAsReadModels
    {
      private readonly List<dynamic> _data;
      
      public InMemoryReadModelStore()
      {
          _data = new List<object>();
      }
        
      public void SaveChangesBy(AggregateEvent aggregateEvent)
      {
          Handle((dynamic)aggregateEvent);
      }
      
      // This method will call if no one other method is assigned to an AggregateEvent
      void Handle(object unassignedEvent)
      {
          var eventData = ((dynamic)unassignedEvent).EventData;
          throw new ArgumentException("This event is not assigned to this instance, " + eventData.GetType().Name);
      }
      
      // a special event handler method
      void Handle(AggregateEventBag<CustomerCreated> customerCreated)
      {
          Add(new CustomerModel
          {
              Id = customerCreated.EventData.Id,
              Name = customerCreated.EventData.Name
          });
      }
      
      // a lot of more event handler methodes...
      
      public T Retrieve<T>(dynamic query)
      {
          return ExecuteQuery(query);
      }
      
      // A customized method to get data by a query
      CustomerModel ExecuteQuery(GetCustomer query)
      {
        var customer = _data.OfType<CustomerModel>().SingleOrDefault(cust => cust.Id == query.Id);
        return customer;
      }
    }

** Note, the `AggregateEventBag` class derived from `AggregateEvent` and will used by Aggregates to inform a state change. **
Event handler methodes shoud be private to hide the API. It is enought to know there is a Save method.

Feel free to implement a unit of work to save and read data ;-)

#### Context ####

To bind CommandHandler and ReadModel Handler I use a `Context`. This is a artificaly kind to describe bounded context of domain driven design.
Any `Context` binds only two Handler. 
How to implement it? Its very simple:

    public class CustomerContext : Context
    {
        public CustomerContext(CommandHandler commands, ReadModelHandler readModelHandler) 
            : base(commands, readModelHandler)
        {
        }
    }

Ready... This is the fastes way to build a context. But you should create for any Aggregate a Context.

#### Aggregate ####

Aggregates bounds business logic an state of your business. Manage all your domain behaviour in an Aggregate 
but never ever infrastructure. Here a simple Aggregate:

    internal class Customer : Aggregate
    {
        public string Name
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
    
The `FromHistory` Method helps you to get the latest state. It is simpler to use this method instead of an other alternative. All changes of state in the Aggregate will apply by call `ApplyChanges` and the asiigned AggregateEvent. 

#### Commands and Events ####

Against other Frameworks you don't need special marker like IMessage or ICommand to mark a Command and/or Event. Use your own types. It is not necessarily to implements an Id property into any Event but Commands should have it, otherwise the AggregateStore will not find the AggregateEvents ;)

