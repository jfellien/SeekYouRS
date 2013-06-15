using System;

namespace SeekYouRS.Store
{
    /// <summary>
    /// Represents the behaviour of an AggregateStore.
    /// </summary>
    public interface IStoreAggregates
    {
        /// <summary>
        /// Raises if state of Aggreagte has changed by command.
        /// </summary>
        event Action<AggregateEvent> AggregateHasChanged;
        /// <summary>
        /// Saves the changes of an Aggregate.
        /// </summary>
        /// <typeparam name="T">Type of Aggregate</typeparam>
        /// <param name="aggregate">Aggregate instance</param>
        void Save<T>(T aggregate) where T : Aggregate;
        /// <summary>
        /// Gets an Aggregate by Id.
        /// </summary>
        /// <typeparam name="T">Type of Aggregate</typeparam>
        /// <param name="aggregateId">Id of Aggregate</param>
        /// <returns></returns>
        T GetAggregate<T>(Guid aggregateId) where T : Aggregate, new();

    }
}