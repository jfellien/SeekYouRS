using System;
using System.Threading.Tasks;
using SeekYouRS.Contracts;
using SeekYouRS.EventStore;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class for components who should handle Commands of an Aggregate. 
	/// The handling of a Command should implemented by a HandleThis Metghod for any Command.
	/// </summary>
	public abstract class CommandHandler : IHandleCommands
	{
		AggregateStore _aggregateStore;

		protected AggregateStore AggregateStore { get { return _aggregateStore; } }

		/// <summary>
		/// Calls the concrete HandleThis method of derived class.
		/// </summary>
		/// <param name="command">The Command who will handle</param>
		public void Handle(dynamic command)
		{
			HandleThis(command);
		}
		/// <summary>
		/// Calls the concrete HandleThis method of derived class and returns the result.
		/// </summary>
		/// <param name="command">The Command who will handle</param>
		public TResult Handle<TResult>(dynamic command)
		{
			return HandleThis<TResult>(command);
		}

		/// <summary>
		/// This is the reference to the EventRecorder who record and replay the AggregateEvents
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
			throw new ArgumentException("Unknown Command detected: " 
				+ command.GetType().Name 
				+ " You should implement a HandleThis Metohd for your Command.");
		}
		/// <summary>
		/// Fallback method to handle unassigned Commands
		/// </summary>
		/// <param name="command"></param>S
		public TResult HandleThis<TResult>(object command)
		{
			throw new ArgumentException("Unknown Command detected: "
				+ command.GetType().Name
				+ " You should implement a HandleThis Metohd for your Command.");
		}
	}
}