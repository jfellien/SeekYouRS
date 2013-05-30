using SeekYouRS.Messaging;

namespace SeekYouRS.Tests.TestObjects
{
    public class CustomerApi : Api
    {
        public CustomerApi(IExecuteCommands commandHandler, IRetrieveModels queriesHandler) 
            : base(commandHandler, queriesHandler)
        {
            commandHandler.Performed += queriesHandler.HandleChanges;
        }
    }
}