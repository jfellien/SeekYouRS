using SeekYouRS.Handler;
using SeekYouRS.Store;

namespace SeekYouRS.Tests.TestObjects
{
    public class CustomerContext : Context
    {
        public CustomerContext(CommandHandler commands, IQueryReadModels queries, ReadModelHandler readModelHandler) 
            : base(commands, queries, readModelHandler)
        {
        }
    }
}