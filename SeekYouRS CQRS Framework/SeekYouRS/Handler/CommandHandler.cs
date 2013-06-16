using System;
using SeekYouRS.Store;

namespace SeekYouRS.Handler
{
	public abstract class CommandHandler : IExecuteCommands
	{
		public event Action<AggregateEvent> HasPerformed;

		protected CommandHandler(IStoreAggregates aggregateStore)
		{
			AggregateStore = aggregateStore;
			AggregateStore.AggregateHasChanged += OnPublished;
		}

		protected IStoreAggregates AggregateStore { get; private set; }

		private void OnPublished(AggregateEvent aggregateEvent)
		{
			if (HasPerformed != null)
				HasPerformed(aggregateEvent);
		}

		public abstract void Process(dynamic command);

		internal void Execute(object command)
		{
			throw new ArgumentException("Unnown Command detected: " + command.GetType().Name);
		}

	}
}