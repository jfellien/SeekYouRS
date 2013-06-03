﻿using SeekYouRS.Messaging;

namespace SeekYouRS
{
    /// <summary>
    /// Base class for CQRS Api components. 
    /// Api connects the Command and Query Handler
    /// </summary>
    public abstract class Api
    {
        private readonly IExecuteCommands _commandHandler;
        private readonly IRetrieveModels _queriesHandler;

        protected Api(IExecuteCommands commandHandler, IRetrieveModels queriesHandler)
        {
            _commandHandler = commandHandler;
            _queriesHandler = queriesHandler;
        }
        /// <summary>
        /// Passed the command to the Command Handler for processing the command.
        /// </summary>
        /// <param name="command">The Command to process</param>
        public void Process(dynamic command)
        {
            _commandHandler.Process(command);
        }
        /// <summary>
        /// Gets the result of query
        /// </summary>
        /// <typeparam name="T">Type of expected result</typeparam>
        /// <param name="query">Query parameters</param>
        /// <returns>Query result</returns>
        public T Retrieve<T>(dynamic query)
        {
            return (T)_queriesHandler.Execute<T>(query);
        }
    }
}
