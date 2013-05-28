using SeekYouRS.Messaging;

namespace SeekYouRS
{
    public abstract class Api
    {
        private readonly IExecuteCommands _commandHandler;
        private readonly QueriesRepository _queriesRepository;

        protected Api(IExecuteCommands commandHandler, QueriesRepository queriesRepository)
        {
            _commandHandler = commandHandler;
            _queriesRepository = queriesRepository;
        }

        public void Process(dynamic command)
        {
            _commandHandler.Process(command);
        }

        public TViewModel ExecuteQuery<TViewModel>(dynamic query)
        {
            return _queriesRepository.Execute<TViewModel>(query);
        }
    }
}
