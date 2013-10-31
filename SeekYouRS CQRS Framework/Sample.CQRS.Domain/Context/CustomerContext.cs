using Sample.CQRS.Domain.CommandHandler;
using Sample.CQRS.ReadModel.EventHandler;
using Sample.CQRS.ReadModel.QueryHandler;
using SeekYouRS.BaseComponents;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace Sample.CQRS.Domain.Context
{
    public class CustomerContext : DomainContext<CustomerCommands, CustomerQueryHandler, CustomerEventHandler>
    {
        public CustomerContext(
            EventRecorder eventRecorder,
            IStoreAndRetrieveReadModels readModelStore)
            : base(eventRecorder, readModelStore) {}
    }
}