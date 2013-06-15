using SeekYouRS.Handler;

namespace SeekYouRS
{
    /// <summary>
    /// Base class for CQRS Context components.
    /// Context combines 
    /// </summary>
    public abstract class Context
    {
        private readonly IExecuteCommands _commands;
        private readonly ReadModelHandler _readModelHandler;

        protected Context(IExecuteCommands commands, ReadModelHandler readModelHandler)
        {
            _commands = commands;
            _readModelHandler = readModelHandler;

            _commands.HasPerformed += _readModelHandler.ApplyChanges;
        }
        /// <summary>
        /// Passed the command to the Command Handler for processing the command.
        /// </summary>
        /// <param name="command">The Command to process</param>
        public void Process(dynamic command)
        {
            _commands.Process(command);
        }
        /// <summary>
        /// Gets the result of query
        /// </summary>
        /// <typeparam name="T">Type of expected result</typeparam>
        /// <param name="query">Query parameters</param>
        /// <returns>Query result</returns>
        public T Retrieve<T>(dynamic query)
        {
            return (T)_readModelHandler.Execute<T>(query);
        }
    }
}
