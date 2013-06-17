using System;
using SeekYouRS.Store;

namespace SeekYouRS.Handler
{
	public abstract class CommandHandler : IExecuteCommands
	{
		public event Action<AggregateEvent> HasPerformed;

		protected CommandHandler(Aggregates aggregatesRepository)
		{
			AggregateStore = aggregatesRepository;
			AggregateStore.AggregateHasChanged += OnPublished;
		}

		protected Aggregates AggregateStore { get; private set; }

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