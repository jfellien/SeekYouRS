using System.Collections.Generic;
using SeekYouRS.Handler;
using SeekYouRS.Store;

namespace SeekYouRS
{
	/// <summary>
	/// Base class for CQRS Context components.
	/// Context combines 
	/// </summary>
	public abstract class Context
	{
		private readonly IExecuteCommands _commands;
		readonly IQueryReadModels _queries;

		protected Context(IExecuteCommands commands, IQueryReadModels queries, IHandleAggregateEvents eventHandler)
		{
			_commands = commands;
			_queries = queries;

			_commands.HasPerformed += eventHandler.SaveChangesBy;
		}

		protected Context(IExecuteCommands commands, IQueryReadModels queries, IEnumerable<IHandleAggregateEvents> eventHandlers)
		{
			_commands = commands;
			_queries = queries;

			foreach (var eventHandler in eventHandlers)
			{
				_commands.HasPerformed += eventHandler.SaveChangesBy;
			}
		}

		/// <summary>
		/// Passed the command to the Command Handler for processing the command.
		/// </summary>
		/// <param name="command">The Command to process</param>
		public void Process(dynamic command)
		{
			_commands.Process(command);
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
