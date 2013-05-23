using SeekYouRS.Messaging;

namespace SeekYouRS.Tests
{
    public class KundenApi : Api
    {
        public KundenApi(IRecieveCommands reciever, IExecuteQueries repository) 
            : base(reciever, repository)
        {
            
        }
    }
}