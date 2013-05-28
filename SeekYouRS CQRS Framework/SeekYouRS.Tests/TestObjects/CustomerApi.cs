using SeekYouRS.Messaging;

namespace SeekYouRS.Tests.TestObjects
{
    public class CustomerApi : Api
    {
        public CustomerApi(IExecuteCommands commandHandler, QueriesRepository queriesRepository) 
            : base(commandHandler, queriesRepository)
        {
            commandHandler.Performed += queriesRepository.HandleChanges;
        }
    }
}