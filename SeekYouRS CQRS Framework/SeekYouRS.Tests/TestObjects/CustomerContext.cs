using System.Collections.Generic;
using SeekYouRS.Handler;
using SeekYouRS.Store;

namespace SeekYouRS.Tests.TestObjects
{
	public class CustomerContext : Context
	{
		public CustomerContext(
			IHandleCommands commands,
			IQueryReadModels queries,
			IEnumerable<IHandleAggregateEvents> eventHandlers)
			: base(commands, queries, eventHandlers)
		{
		}

		public CustomerContext(
			IHandleCommands commands,
			IQueryReadModels queries,
			IHandleAggregateEvents eventHandler)
			: base(commands, queries, eventHandler)
		{
		}
	}
}