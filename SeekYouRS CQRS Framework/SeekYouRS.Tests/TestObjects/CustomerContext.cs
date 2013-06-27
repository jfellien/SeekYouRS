using System.Collections.Generic;
using SeekYouRS.Contracts;

namespace SeekYouRS.Tests.TestObjects
{
	public class CustomerContext : Context
	{
		public CustomerContext(
			IExecuteCommands commands,
			IQueryReadModels queries,
			IEnumerable<IHandleAggregateEvents> eventHandlers)
			: base(commands, queries, eventHandlers)
		{
		}

		public CustomerContext(
			IExecuteCommands commands,
			IQueryReadModels queries,
			IHandleAggregateEvents eventHandler)
			: base(commands, queries, eventHandler)
		{
		}
	}
}