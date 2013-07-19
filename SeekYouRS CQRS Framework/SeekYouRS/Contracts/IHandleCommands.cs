using System;
using System.Threading.Tasks;
using SeekYouRS.EventStore;

namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Describes the behaviour of a CommandHandler. 
	/// Use CommandHandler class as base for your projects. It contains all base features.
	/// </summary>
	public interface IHandleCommands
	{
		/// <summary>
		/// Starts the processing of a command
		/// </summary>
		/// <param name="command">Object with parameters of command</param>
		void Handle(dynamic command);
		/// <summary>
		/// Starts the processing of a command
		/// </summary>
		/// <param name="command">Object with parameters of command</param>
		TResult Handle<TResult>(dynamic command);

		/// <summary>
		/// Sets the EventRecorder who record and replay the AggregateEvents
		/// </summary>
		EventRecorder EventRecorder { set; }
	}
}