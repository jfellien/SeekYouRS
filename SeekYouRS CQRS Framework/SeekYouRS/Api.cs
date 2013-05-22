using SeekYouRS.Messaging;

namespace SeekYouRS
{
    public abstract class Api
    {
        protected Api(IRecieveCommands reciever, IExecuteQueries repository)
        {
            Commands = reciever;
            Repository = repository;
        }

        public IExecuteQueries Repository { get; private set; }

        public IRecieveCommands Commands { get; private set; }
    }
}
