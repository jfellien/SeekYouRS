using System;

namespace SeekYouRS.Handler
{
	/// <summary>
	/// Represents the behaviour of a Command Handler. 
	/// Use CommandHandler class as base for your projects. It contains all base features.
	/// </summary>
	public interface IHandleCommands
	{
		/// <summary>
		/// Starts the processing of a command
		/// </summary>
		/// <param name="command">Object with parameters of command</param>
		void Process(dynamic command);

		/// <summary>
		/// Raises if the process of command is ready
		/// </summary>
		event Action<AggregateEvent> HasPerformed;
	}
}