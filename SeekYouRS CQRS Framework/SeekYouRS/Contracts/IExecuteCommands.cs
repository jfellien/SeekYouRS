using System;

namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Represents the behaviour of a Command Handler. 
	/// Use Commands class as base for your projects. It contains all base features.
	/// </summary>
	public interface IExecuteCommands
	{
		/// <summary>
		/// Starts the processing of a command
		/// </summary>
		/// <param name="command">Object with parameters of command</param>
		void Execute(dynamic command);

		/// <summary>
		/// Raises if the process of command is ready
		/// </summary>
		event Action<AggregateEvent> HasPerformed;
	}
}