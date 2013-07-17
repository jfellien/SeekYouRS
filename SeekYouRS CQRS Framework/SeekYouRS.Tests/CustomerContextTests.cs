using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;
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
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			api.Process(new CreateCustomer{Id = id, Name = "My Customer"});

			var customerModel = api.ExecuteQuery<CustomerModel>(new GetCustomer {Id = id});

			customerModel.Name.ShouldBeEquivalentTo("My Customer");
		}

		[Test]
		public void TestToCreateAndChangeKundeViaApi()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
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
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

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
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

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
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

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
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			Assert.Catch<ArgumentException>(() => api.Process(new UnknownCommand()));
		}

		[Test]
		public void TestToGetAnExceptionIfAggregateEventIsUnknown()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			Assert.Catch<ArgumentException>(() => api.Process(new CommandWithoutEventHandling()));
		}

		[Test]
		public void TestToGetAnExceptionIfQueryIsUnknown()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			
			Assert.Catch<ArgumentException>(() => api.ExecuteQuery<CustomerModel>(new UnknownQuery()));
		}
	}
}