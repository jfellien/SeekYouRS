using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class for CQRS DomainContext components.
	/// DomainContext combines 
	/// </summary>
	public abstract class DomainContext<TCommandHandler, TQueriesHandler, TAggregateEventHandler> 
		where TCommandHandler : CommandHandler, new()
		where TQueriesHandler : QueryHandler, new()
		where TAggregateEventHandler : EventHandler, new()
	{
		readonly CommandHandler _commandHandler;
		readonly QueryHandler _queryHandler;
		readonly EventHandler _eventHandler;

		protected DomainContext(EventRecorder eventRecorder, IStoreAndRetrieveReadModels readModelStore)
		{
			_commandHandler = new TCommandHandler
				{
					EventRecorder = eventRecorder
				};

			_queryHandler = new TQueriesHandler
				{
					ReadModelStore = readModelStore
				};

			_eventHandler = new TAggregateEventHandler
				{
					ReadModelStore = readModelStore
				};

			eventRecorder.EventHasStored += _eventHandler.Handle;
		}

		/// <summary>
		/// Passed the command to the Command Handler for processing the command.
		/// </summary>
		/// <param name="command">The Command to process</param>
		public void Process(dynamic command)
		{
			_commandHandler.Handle(command);
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
