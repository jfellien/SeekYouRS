using System;
using SeekYouRS.Store;

namespace SeekYouRS.Handler
{
	/// <summary>
	/// Base calss for CommandHandler.
	/// This handles Commands who informs the Aggregates to change the state.
	/// After Handling the Handler informs a Subscriber about state changes.
	/// </summary>
	public abstract class CommandHandler : IHandleCommands
	{
		/// <summary>
		/// Raises if Command is ready and the state of an Aggregate has changed.
		/// </summary>
		public event Action<AggregateEvent> HasPerformed;

		protected CommandHandler(IStoreAndRetrieveAggregateEvents aggregateEventsStore)
		{
			SetupAggregateStore(aggregateEventsStore);
		}

		void SetupAggregateStore(IStoreAndRetrieveAggregateEvents aggregateEventsStore)
		{
			AggregateStore = new Aggregates(aggregateEventsStore);
			AggregateStore.AggregateHasChanged += OnPublished;
		}

		/// <summary>
		/// Gets the AggregateStore who saves the state changes
		/// </summary>
		protected Aggregates AggregateStore { get; private set; }

		void OnPublished(AggregateEvent aggregateEvent)
		{
			if (HasPerformed != null)
				HasPerformed(aggregateEvent);
		}

		/// <summary>
		/// Derived method from interface IHandleCommands.
		/// You should implement this Method by calling 'Handle(comand)' and 
		/// implement for any Command an Handle method.
		/// </summary>
		/// <param name="command">The Command who will handle</param>
		public abstract void Process(dynamic command);

		/// <summary>
		/// Fallback method to handle unassigned Commands
		/// </summary>
		/// <param name="command"></param>
		public void Handle(object command)
		{
			throw new ArgumentException("Unnown Command detected: " + command.GetType().Name);
		}

	}
}