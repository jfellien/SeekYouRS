using System;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class for CommandHandler.
	/// This handles Commands who informs the Aggregates to change the state.
	/// </summary>
	public abstract class CommandHandler
	{
		/// <summary>
		/// Instance of a store who saves and restores Aggregates 
		/// </summary>
		AggregateStore _aggregateStore;

		/// <summary>
		/// Gets the AggregateStore.
		/// </summary>
		protected AggregateStore AggregateStore { get { return _aggregateStore; } }

		/// <summary>
		/// This method calls the concrete implementation of Handling Method.
		/// You should implement this method by calling 'HandleThis(command)' and 
		/// implement for any Command an HandleThis method.
		/// </summary>
		/// <param name="command">The Command who will handle</param>
		public abstract void Handle(dynamic command);

		/// <summary>
		/// This method calls the concrete implementation of Handling Method and returns a result.
		/// You should implement this method by calling 'HandleThis(command)' and 
		/// implement for any Command an HandleThis method.
		/// </summary>
		/// <param name="command">The Command who will handle</param>
		public abstract TResult Handle<TResult>(dynamic command);

		/// <summary>
		/// Sets the EventRecorder who record and replay AggregateEvents
		/// </summary>
		public EventRecorder EventRecorder
		{
			set
			{
				if (_aggregateStore == null)
				{
					_aggregateStore = new AggregateStore(value);
				}
			}
		}

		/// <summary>
		/// Fallback method to handle unassigned Commands
		/// </summary>
		/// <param name="command"></param>
		public void HandleThis(object command)
		{
			throw new ArgumentException("Unnown Command detected: " + command.GetType().Name);
		}
	}
}