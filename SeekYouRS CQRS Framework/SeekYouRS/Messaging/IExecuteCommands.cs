using System;
using SeekYouRS.Storing;

namespace SeekYouRS.Messaging
{
    /// <summary>
    /// Defined the behaviour of a Command Handler 
    /// </summary>
    public interface IExecuteCommands
    {
        /// <summary>
        /// Starts processing the command.
        /// </summary>
        /// <param name="command"></param>
        void Process(dynamic command);
        /// <summary>
        /// Raises if the process of command is ready.
        /// </summary>
        event Action<AggregateEvent> Performed;
    }
}