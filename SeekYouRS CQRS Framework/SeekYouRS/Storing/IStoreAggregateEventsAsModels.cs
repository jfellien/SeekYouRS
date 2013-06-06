using System.Linq;

namespace SeekYouRS.Storing
{
    /// <summary>
    /// Defined the behaviour of a ModelStore. 
    /// This store saves any kind of objects. 
    /// The implementation should use to store the Models eg.ViewModels.
    /// </summary>
    public interface IStoreAggregateEventsAsModels
    {
        /// <summary>
        /// Gets a single instance of model by query parameters.
        /// </summary>
        /// <typeparam name="T">Type of Model</typeparam>
        /// <param name="query">Query parameters</param>
        /// <returns>Single instance</returns>
        T Retrieve<T>(dynamic query);

        /// <summary>
        /// Handles changes by Aggregate Events
        /// </summary>
        /// <param name="aggregateEvent"></param>
        void HandleChanges(AggregateEvent aggregateEvent);
    }
}