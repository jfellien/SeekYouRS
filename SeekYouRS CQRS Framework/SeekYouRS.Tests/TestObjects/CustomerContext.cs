using SeekYouRS.Handler;

namespace SeekYouRS.Tests.TestObjects
{
    public class CustomerContext : Context
    {
        public CustomerContext(CommandHandler commands, ReadModelHandler readModelHandler) 
            : base(commands, readModelHandler)
        {
        }
    }
}