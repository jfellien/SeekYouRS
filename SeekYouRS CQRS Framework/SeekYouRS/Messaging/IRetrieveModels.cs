using SeekYouRS.Storing;

namespace SeekYouRS.Messaging
{
    /// <summary>
    /// Defined the beahaviour of a Read Model Handler
    /// </summary>
    public interface IRetrieveModels
    {
        /// <summary>
        /// Exececutes a query and returns a single instance of Model
        /// </summary>
        /// <typeparam name="T">Expected type of Model. Lists and singel Modles allowed.</typeparam>
        /// <param name="query">Instance of query parameters</param>
        /// <returns></returns>
        T Execute<T>(dynamic query);
        /// <summary>
        /// Handles changes by Aggregate Events
        /// </summary>
        /// <param name="aggregateEvent"></param>
        void HandleChanges(AggregateEvent aggregateEvent);
    }
}
