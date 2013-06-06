using SeekYouRS.Messaging;

namespace SeekYouRS.Tests.TestObjects
{
    public class CustomerContext : Context
    {
        public CustomerContext(IExecuteCommands commandHandler, IRetrieveModels queriesHandler) 
            : base(commandHandler, queriesHandler)
        {
        }
    }
}