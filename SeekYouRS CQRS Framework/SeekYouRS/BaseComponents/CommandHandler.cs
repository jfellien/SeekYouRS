using System;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class for CommandHandler.
	/// This handles Commands who informs the Aggregates to change the state.
	/// After Handling the Handler informs a Subscriber about state changes.
	/// </summary>
	public abstract class CommandHandler
	{
		AggregateStore _aggregateStore;

		protected AggregateStore AggregateStore { get { return _aggregateStore; } }

		/// <summary>
		/// Derived method from interface IHandleCommands.
		/// You should implement this Method by calling 'Handle(comand)' and 
		/// implement for any Command an Handle method.
		/// </summary>
		/// <param name="command">The Command who will handle</param>
		public abstract void Handle(dynamic command);

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