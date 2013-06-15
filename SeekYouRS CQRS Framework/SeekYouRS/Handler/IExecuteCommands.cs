using System;

namespace SeekYouRS.Handler
{
    /// <summary>
    /// Represents the behaviour of a Command Handler 
    /// </summary>
    public interface IExecuteCommands
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