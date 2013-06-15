namespace SeekYouRS.Store
{
    /// <summary>
    /// Represents the behaviour of a ReadModelHandler who can save changes.
    /// The changes effected by AggregateEvents.
    /// </summary>
    public interface IStoreAggregateEventsAsReadModels
    {
        /// <summary>
        /// Gets the Model by query
        /// </summary>
        /// <typeparam name="T">Type of Model</typeparam>
        /// <param name="query">Query parameters</param>
        /// <returns>Instance of Model</returns>
        T Retrieve<T>(dynamic query);

        /// <summary>
        /// Save the changes by an AggregateEvent
        /// </summary>
        /// <param name="aggregateEvent">The AggregateEvent who contains the parameters of change</param>
        void SaveChangesBy(AggregateEvent aggregateEvent);
    }
}