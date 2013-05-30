using SeekYouRS.Messaging;

namespace SeekYouRS
{
    public abstract class Api
    {
        private readonly IExecuteCommands _commandHandler;
        private readonly IRetrieveModels _queriesHandler;

        protected Api(IExecuteCommands commandHandler, IRetrieveModels queriesHandler)
        {
            _commandHandler = commandHandler;
            _queriesHandler = queriesHandler;
        }

        public void Process(dynamic command)
        {
            _commandHandler.Process(command);
        }

        public TModel Retrieve<TModel>(dynamic query)
        {
            return (TModel)_queriesHandler.Execute<TModel>(query);
        }
    }
}
