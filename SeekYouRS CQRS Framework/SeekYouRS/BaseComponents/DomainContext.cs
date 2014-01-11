using System;
using System.Threading.Tasks;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class for CQRS DomainContext components.
	/// DomainContext combines 
	/// </summary>
	/// <typeparam name="TCommandHandler">Type of concrete CommandHandler</typeparam>
	/// <typeparam name="TQueriesHandler">Type of concrete QueriesHandler</typeparam>
	/// <typeparam name="TAggregateEventHandler">Type of concrete EventHandler</typeparam>
	public abstract class DomainContext<TCommandHandler, TQueriesHandler, TAggregateEventHandler> : DomainContext
		where TCommandHandler : CommandHandler, new()
		where TQueriesHandler : QueryHandler, new()
		where TAggregateEventHandler : EventHandler, new()
	{
		protected DomainContext(EventRecorder eventRecorder, IStoreAndRetrieveReadModels readModelStore) 
            : base(new TCommandHandler(), new TQueriesHandler(), new TAggregateEventHandler(), 
            eventRecorder,readModelStore)
		{ }

	}

    public abstract class DomainContext
    {
        protected readonly CommandHandler _commandHandler;
        protected readonly QueryHandler _queryHandler;
        protected readonly EventHandler _eventHandler;

        public DomainContext(CommandHandler commandHandler, 
            QueryHandler queryHandler, 
            EventHandler eventHandler, 
            EventRecorder eventRecorder, 
            IStoreAndRetrieveReadModels readModelStore)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
            _eventHandler = eventHandler;

            SetupHandler(eventRecorder, readModelStore);

            eventRecorder.EventHasStored += _eventHandler.Handle;
        }

        private void SetupHandler(EventRecorder eventRecorder, IStoreAndRetrieveReadModels readModelStore)
        {
            _commandHandler.EventRecorder = eventRecorder;
            _queryHandler.ReadModelStore = readModelStore;
            _eventHandler.ReadModelStore = readModelStore;
        }

        /// <summary>
        /// Passed the command to the Command Handler for processing the command.
        /// </summary>
        /// <param name="command">The Command to process</param>
        public Task Process(dynamic command)
        {
            Action<dynamic> handle = cmd => _commandHandler.Handle(cmd);

            return Task.Factory.StartNew(handle, command);
        }

        /// <summary>
        /// Passed the command to the Command Handler for processing the command.
        /// </summary>
        /// <param name="command">The Command to process</param>
        public TResult Process<TResult>(dynamic command)
        {
            return _commandHandler.Handle<TResult>(command);
        }

        /// <summary>
        /// Gets the result of query
        /// </summary>
        /// <typeparam name="T">Type of expected result</typeparam>
        /// <param name="query">Query parameters</param>
        /// <returns>Query result</returns>
        public T ExecuteQuery<T>(dynamic query)
        {
            return (T)_queryHandler.Retrieve<T>(query);
        }
    }
}
