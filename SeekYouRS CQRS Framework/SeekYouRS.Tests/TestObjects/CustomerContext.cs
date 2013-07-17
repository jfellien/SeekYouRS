using SeekYouRS.BaseComponents;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;
using SeekYouRS.Tests.TestObjects.Handler;
using SeekYouRS.Tests.TestObjects.Queries;

namespace SeekYouRS.Tests.TestObjects
{
	public class CustomerContext : DomainContext<CustomerCommands, CustomerQueries, CustomerAggregateEventHandler>
	{
		public CustomerContext(
			EventRecorder eventRecorder, 
			IStoreAndRetrieveReadModels readModelStore) 
			: base(eventRecorder, readModelStore)
		{
		}
	}
}