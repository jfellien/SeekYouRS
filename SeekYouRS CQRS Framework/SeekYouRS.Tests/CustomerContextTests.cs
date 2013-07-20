using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.EventStore;
using SeekYouRS.Tests.TestObjects;
using SeekYouRS.Tests.TestObjects.Commands;
using SeekYouRS.Tests.TestObjects.Models;
using SeekYouRS.Tests.TestObjects.Queries;

namespace SeekYouRS.Tests
{
	[TestFixture]
	internal class CustomerContextTests
	{
		[Test]
		public async void TestToCreateCustomerAndReadIt()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			await api.Process(new CreateCustomer{Id = id, Name = "My Customer"});
		
			var customerModel = api.ExecuteQuery<CustomerModel>(new GetCustomer { Id = id });

			Trace.WriteLine(customerModel);
			Console.WriteLine(customerModel);

			customerModel.Name.ShouldBeEquivalentTo("My Customer");
		}

		[Test]
		public async void TestToCreateAndChangeCustomerViaApi()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			await api.Process(new CreateCustomer {Id = id, Name = "My Customer"});

			await api.Process(new ChangeCustomer {Id = id, Name = "New Name"});

			var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
				{
					Id = id
				});

			customer.Name.ShouldBeEquivalentTo("New Name");
		}

		[Test]
		public async void TestToReturnsAStringValueFromDomain()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			await api.Process(new CreateCustomer {Id = id, Name = "My Customer"});

			var commandResult = api.Process<String>(new GetStringResult {Id = id, ExpectedResult = "Test"});

			commandResult.ShouldBeEquivalentTo("Test");
		}

		[Test]
		public async void TestToReturnsAIntegerValueFromDomain()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			await api.Process(new CreateCustomer {Id = id, Name = "My Customer"});
			var commandResult = api.Process<int>(new GetIntResult {Id = id, ExpectedResult = 12});

			commandResult.ShouldBeEquivalentTo(12);
		}

		[Test]
		public async void TestToCreateTwoCustomerAndChangeOneOfThem()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			await api.Process(new CreateCustomer {Id = id1, Name = "Customer One"});

			await api.Process(new CreateCustomer {Id = id2, Name = "Customer Two"});
			await api.Process(new ChangeCustomer {Id = id2, Name = "Customer Two Changed"});

			var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
				{
					Id = id2
				});

			customer.Name.ShouldBeEquivalentTo("Customer Two Changed");
		}

		[Test]
		public async void TestToRemoveACustomer()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var id = Guid.NewGuid();

			await api.Process(new CreateCustomer {Id = id, Name = "Customer To Remove"});

			var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
				{
					Id = id
				});

			customer.Should().NotBeNull();

			await api.Process(new RemoveCustomer {Id = id});

			customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
				{
					Id = id
				});

			customer.Should().BeNull();
		}

		[Test]
		public async void TestToGetAListOfCustomers()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			await api.Process(new CreateCustomer {Id = id1, Name = "Customer One"});
			await api.Process(new CreateCustomer {Id = id2, Name = "Customer Two"});

			var customers = api.ExecuteQuery<IEnumerable<CustomerModel>>(new GetAllCustomers()).ToList();

			customers.Count().ShouldBeEquivalentTo(2);

			customers.Should().Contain(c => c.Id == id1);
			customers.Should().Contain(c => c.Id == id2);

		}

		[Test]
		public void TestToGetAnExceptionIfACommandUnkown()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var exceptionShouldThrown = api.Process(new UnknownCommand());
			exceptionShouldThrown.ContinueWith(task => task.Exception.Should().NotBeNull());
		}

		[Test]
		public void TestToGetAnExceptionIfAggregateEventIsUnknown()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var exceptionShouldThrown = api.Process(new CommandWithoutEventHandling());
			exceptionShouldThrown.ContinueWith(task => task.Exception.Should().NotBeNull());
		}

		[Test]
		public void TestToGetAnExceptionIfQueryIsUnknown()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			Assert.Catch<ArgumentException>(() => api.ExecuteQuery<CustomerModel>(new UnknownQuery()));
		}

		[Test]
		public void TestALongRunnigCommand()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			var beforeStart = DateTime.Now;

			api.Process(new StartLongRunningProcess { Id = id, Milliseconds = 5000 });
			var afterStart = DateTime.Now;
			var timeDiff = afterStart - beforeStart;

			timeDiff.Milliseconds.Should().BeLessThan(5000);
			Console.WriteLine(timeDiff.Milliseconds);
		}
	}
}