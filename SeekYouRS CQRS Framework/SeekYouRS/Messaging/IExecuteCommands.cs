using System;
using SeekYouRS.Storing;

namespace SeekYouRS.Messaging
{
    public interface IExecuteCommands
    {
        void Process(dynamic command);
        event Action<AggregateEvent> Performed;
    }
}