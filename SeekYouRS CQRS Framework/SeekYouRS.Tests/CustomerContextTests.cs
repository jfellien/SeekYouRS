using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

			var createCustomer = api.Process(new CreateCustomer{Id = id, Name = "My Customer"});

			createCustomer.ContinueWith(task =>
				{
					var customerModel = api.ExecuteQuery<CustomerModel>(new GetCustomer { Id = id });

					customerModel.Name.ShouldBeEquivalentTo("My Customer");
				});
		}

		[Test]
		public void TestToCreateAndChangeKundeViaApi()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			var createCustomer = api.Process(new CreateCustomer { Id = id, Name = "My Customer" });

			createCustomer.ContinueWith(task =>
				{
					var changeCustomer = api.Process(new ChangeCustomer {Id = id, Name = "New Name"});

					changeCustomer.ContinueWith(task1 =>
						{
							var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
							{
								Id = id
							});

							customer.Name.ShouldBeEquivalentTo("New Name");

						});
				});
		}

		[Test]
		public void TestToCreateTwoCustomerAndChangeOneOfThem()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			var createCustomer1 = api.Process(new CreateCustomer { Id = id1, Name = "Customer One" });

			createCustomer1.ContinueWith(task =>
				{
					var createCustomer2 = api.Process(new CreateCustomer { Id = id2, Name = "Customer Two" });

					createCustomer2.ContinueWith(task1 =>
						{


							var changeCustomer2 = api.Process(new ChangeCustomer {Id = id2, Name = "Customer Two Changed"});

							changeCustomer2.ContinueWith(task2 =>
								{


									var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
										{
											Id = id2
										});

									customer.Name.ShouldBeEquivalentTo("Customer Two Changed");
								});
						});
				});
			

		}

		[Test]
		public void TestToRemoveACustomer()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var id = Guid.NewGuid();

			var createCustomer = api.Process(new CreateCustomer { Id = id, Name = "Customer To Remove" });

			createCustomer.ContinueWith(task =>
				{
					var customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
						{
							Id = id
						});

					customer.Should().NotBeNull();

					var removeCustomer = api.Process(new RemoveCustomer { Id = id });

					removeCustomer.ContinueWith(task1 =>
						{
							customer = api.ExecuteQuery<CustomerModel>(new GetCustomer
								{
									Id = id
								});

							customer.Should().BeNull();

						});
				});
		}

		[Test]
		public void TestToGetAListOfCustomers()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			var createCustomer1 = api.Process(new CreateCustomer { Id = id1, Name = "Customer One" });

			createCustomer1.ContinueWith(task =>
				{
					var createCustomer2 = api.Process(new CreateCustomer {Id = id2, Name = "Customer Two"});
					createCustomer2.ContinueWith(task1 =>
						{
							var customers = api.ExecuteQuery<IEnumerable<CustomerModel>>(new GetAllCustomers()).ToList();

							customers.Count().ShouldBeEquivalentTo(2);

							customers.Should().Contain(c => c.Id == id1);
							customers.Should().Contain(c => c.Id == id2);
						});
				});
		}

		[Test]
		public void TestToCreateAndGetResultValueOfTypeString()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			var createCustomer = api.Process(new CreateCustomer { Id = id, Name = "My Customer" });

			createCustomer.ContinueWith(task =>
			{
				var resultString = api.Process<String>(new GetStringResult { Id = id, Name = "New Name" });

				resultString.ContinueWith(task1 => task1.Result.ShouldBeEquivalentTo("New Name"));
			});
		}

		[Test]
		public void TestToCreateAndGetResultValueOfTypeInt()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			var createCustomer = api.Process(new CreateCustomer { Id = id, Name = "My Customer" });

			createCustomer.ContinueWith(task =>
			{
				var resultString = api.Process<int>(new GetIntResult { Id = id, Number = 12 });

				resultString.ContinueWith(task1 => task1.Result.ShouldBeEquivalentTo(12));
			});
		}

		[Test]
		public void TestToGetAnExceptionIfACommandUnkown()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);

			var exception = api.Process(new UnknownCommand());

			exception.ContinueWith(task =>
				{
					Assert.IsNotNullOrEmpty(task.Exception.Message);
				});
		}

		[Test]
		public void TestToGetAnExceptionIfAggregateEventIsUnknown()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var exception = api.Process(new CommandWithoutEventHandling());
			exception.ContinueWith(task =>
				{
					Assert.IsNotNullOrEmpty(task.Exception.Message);
				});
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
		public void TestALongRunningCommand()
		{
			var eventRecorder = new EventRecorder(new InMemoryAggregateEventStore());
			var readModelStore = new InMemoryReadModelStore();

			var api = new CustomerContext(eventRecorder, readModelStore);
			var id = Guid.NewGuid();

			api.Process(new CreateCustomer { Id = id, Name = "My Customer" });

			var startTime = DateTime.Now;

			api.Process(new LongRunningCommand {Id = id});

			var endTime = DateTime.Now;

			var timeDiff = endTime - startTime;

			Assert.LessOrEqual(timeDiff.Seconds, 2);
		}
	}
}