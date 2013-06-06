using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using SeekYouRS.Storing;

namespace SeekYouRS
{
    /// <summary>
    /// Base class of an Aggregate.
    /// Aggregates collects Entities and Value Objects of a specific domain context.
    /// </summary>
    public abstract class Aggregate
    {
        protected Aggregate()
        {
            Changes = new List<AggregateEvent>();
        }

        /// <summary>
        /// Gets the id of the current Aggregeate. Derived class should implements the method to read Id from history.
        /// </summary>
        public abstract Guid Id { get; }

        /// <summary>
        /// Gets the list of current changes. List will reset after successful saving.
        /// </summary>
        public IList<AggregateEvent> Changes { get; private set; }
        /// <summary>
        /// Gets or sets the list of all status changes
        /// </summary>
        public IEnumerable<AggregateEvent> History { get; set; }

        /// <summary>
        /// Used the Event to change the status of this instance.
        /// </summary>
        /// <typeparam name="T">Type of Event</typeparam>
        /// <param name="changeEvent">Event with change parameters</param>
        protected void ApplyChanges<T>(T changeEvent) where T : class
        {
            var idToSaveChanges = GetIdFrom(changeEvent);

            if (idToSaveChanges == Guid.Empty)
                idToSaveChanges = GetIdFromCurrentAggregate();

            Changes.Add(new AggregateEventBag<T>(idToSaveChanges) { EventData = changeEvent });
        }

        /// <summary>
        /// Gets the Id of Change Event
        /// </summary>
        /// <param name="changeEvent">Event with expected property Id</param>
        /// <returns>returns the id. If Event have not an Id method returns Guid.Empty</returns>
        Guid GetIdFrom(object changeEvent)
        {
            return Reflector.ReadValueOrDefault(changeEvent, "Id", Guid.Empty);
        }

        /// <summary>
        /// Gets the Id from Aggregate.
        /// It is possible that an derived class of Aggregate has no correct implementation of property Id.
        /// So it is necessarily to secure the get of Id.
        /// </summary>
        /// <returns></returns>
        Guid GetIdFromCurrentAggregate()
        {
            try
            {
                return Id;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException(
                "Unable to find a valid Id for Aggregate. Ensure the Event or Aggregate contains an Id.");
            }
        }

        /// <summary>
        /// Gets all Events of specific type
        /// </summary>
        /// <typeparam name="T">Type of Event</typeparam>
        /// <returns>Data of history</returns>
        protected T FromHistory<T>() where T : new()
        {
            var lastEventOfSearchedType = History.Concat(Changes).OfType<AggregateEventBag<T>>().LastOrDefault();
            return lastEventOfSearchedType == null
            ? default(T)
            : lastEventOfSearchedType.EventData;
        }
    }
}