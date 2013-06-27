using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.Contracts;
using SeekYouRS.Tests.TestObjects;
using SeekYouRS.Tests.TestObjects.Commands;
using SeekYouRS.Tests.TestObjects.Events;
using SeekYouRS.Tests.TestObjects.Handler;
using SeekYouRS.Tests.TestObjects.Models;
using SeekYouRS.Tests.TestObjects.Queries;

namespace SeekYouRS.Tests
{
	[TestFixture]
	internal class CustomerContextTests
	{
		[Test]
		public void TestToCreateCustomerAndReadIt()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();

			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);
			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);
			var id = Guid.NewGuid();

			api.Process(new CreateCustomer{Id = id, Name = "My Customer"});

			var customerModel = api.ExecuteQuery<CustomerModel>(new GetCustomer {Id = id});

			customerModel.Name.ShouldBeEquivalentTo("My Customer");
		}

		[Test]
		public void TestToCreateAndChangeKundeViaApi()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);
			var id = Guid.NewGuid();

			api.Process(new CreateCustomer { Id = id, Name = "My Customer" });
			api.Process(new ChangeCustomer { Id = id, Name = "New Name" });

			var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
				{
					Id = id
				});

			customer.Name.ShouldBeEquivalentTo("New Name");
		}

		[Test]
		public void TestToCreateTwoCustomerAndChangeOneOfThem()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);

			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			api.Process(new CreateCustomer { Id = id1, Name = "Customer One" });
			api.Process(new CreateCustomer { Id = id2, Name = "Customer Two" });

			api.Process(new ChangeCustomer { Id = id2, Name = "Customer Two Changed" });

			var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
			{
				Id = id2
			});

			customer.Name.ShouldBeEquivalentTo("Customer Two Changed");

		}

		[Test]
		public void TestToRemoveACustomer()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);

			var id = Guid.NewGuid();

			api.Process(new CreateCustomer { Id = id, Name = "Customer To Remove" });

			var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
			{
				Id = id
			});

			customer.Should().NotBeNull();

			api.Process(new RemoveCustomer { Id = id });

			customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
			{
				Id = id
			});

			customer.Should().BeNull();
		}

		[Test]
		public void TestToGetAListOfCustomers()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);

			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			api.Process(new CreateCustomer { Id = id1, Name = "Customer One" });
			api.Process(new CreateCustomer { Id = id2, Name = "Customer Two" });

			var customers = api.ExecuteQuery<IEnumerable<CustomerModel>>(new GetAllCustomers()).ToList();

			customers.Count().ShouldBeEquivalentTo(2);

			customers.Should().Contain(c => c.Id == id1);
			customers.Should().Contain(c => c.Id == id2);

		}

		[Test]
		public void TestToGetAnExceptionIfACommandUnkown()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);

			Assert.Catch<ArgumentException>(() => api.Process(new UnknownCommand()));
		}

		[Test]
		public void TestToGetAnExceptionIfAggregateEventIsUnknown()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);

			Assert.Catch<ArgumentException>(() => api.Process(new CommandWithoutEventHandling()));
		}

		[Test]
		public void TestToGetAnExceptionIfQueryIsUnknown()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var readModelHandler = new CustomerAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries, readModelHandler);
			
			Assert.Catch<ArgumentException>(() => api.ExecuteQuery<CustomerModel>(new UnknownQuery()));
		}

		[Test]
		public void TestToRecieveAggregateEventsOnMultipleHandler()
		{
			var aggreagteStore = new InMemoryAggregateEventStore();
			var readModelStore = new InMemoryReadModelStore();
			var customerReadModelHandler = new CustomerAggregateEventHandler(readModelStore);
			var vehicleReadModelHandler = new VehicleAggregateEventHandler(readModelStore);

			var commands = new CustomerCommands(aggreagteStore);
			var queries = new CustomerQueries(readModelStore);

			var api = new CustomerContext(commands, queries,
			                              new List<IHandleAggregateEvents>
				                              {
					                              customerReadModelHandler,
					                              vehicleReadModelHandler
				                              });

			var id1 = Guid.NewGuid();

			api.Process(new CreateCustomer { Id = id1, Name = "Customer One" });

			var customers = api.ExecuteQuery<IEnumerable<CustomerModel>>(new GetAllCustomers()).ToList();


		}

	}
}