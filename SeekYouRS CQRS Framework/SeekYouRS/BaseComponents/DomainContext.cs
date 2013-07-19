using System.Threading.Tasks;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class for CQRS DomainContext components.
	/// This Context component combined the CommandHandler and Queries of an Aggregate. 
	/// Also provides this component the communication from Domain to the EventRecorder
	/// </summary>
	public abstract class DomainContext<TCommandHandler, TQueriesHandler, TAggregateEventHandler> 
		where TCommandHandler : IHandleCommands, new()
		where TQueriesHandler : IQueryReadModels, new()
		where TAggregateEventHandler : IHandleAggregateEvents, new()
	{
		readonly IHandleCommands _commands;
		readonly IQueryReadModels _queries;
		readonly IHandleAggregateEvents _aggregateEventHandler;

		protected DomainContext(EventRecorder eventRecorder, IStoreAndRetrieveReadModels readModelStore)
		{
			_commands = new TCommandHandler
				{
					EventRecorder = eventRecorder
				};

			_queries = new TQueriesHandler
				{
					ReadModelStore = readModelStore
				};

			_aggregateEventHandler = new TAggregateEventHandler
				{
					ReadModelStore = readModelStore
				};

			eventRecorder.EventHasStored += _aggregateEventHandler.Handle;
		}

		/// <summary>
		/// Passed the command to the Command Handler for processing the command.
		/// </summary>
		/// <param name="command">The Command to process</param>
		public async Task Process(dynamic command)
		{
			await _commands.Handle(command);
		}
		/// <summary>
		/// Passed the command to the Command Handler for processing the command.
		/// </summary>
		/// <param name="command">The Command to process</param>
		public async Task<TResult> Process<TResult>(dynamic command)
		{
			return await _commands.Handle<TResult>(command);
		}

		/// <summary>
		/// Gets the result of query
		/// </summary>
		/// <typeparam name="T">Type of expected result</typeparam>
		/// <param name="query">Query parameters</param>
		/// <returns>Query result</returns>
		public T ExecuteQuery<T>(dynamic query)
		{
			return (T)_queries.Retrieve<T>(query);
		}
	}
}
