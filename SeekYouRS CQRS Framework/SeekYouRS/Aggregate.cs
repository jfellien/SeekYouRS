using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Storing;

namespace SeekYouRS
{
    public abstract class Aggregate
    {
        protected Aggregate()
        {
            Changes = new List<AggregateEvent>();
        }

        public abstract Guid Id { get; }

        public IList<AggregateEvent> Changes { get; private set; }

        public IEnumerable<AggregateEvent> History { get; set; }

        protected void ApplyChanges<T>(T changeEvent) where T : class
        {
            var id = IdFrom(changeEvent);

            if (id == Guid.Empty)
            {
                try { id = Id; }
                catch (NullReferenceException)
                {
                    throw new ArgumentException(
                        "Unable to find a valid Id for Aggregate. Ensure the Event or Aggregate contains an Id.");
                }
            }

            Changes.Add(new AggregateEventBag<T>(id) { EventData = changeEvent });
        }

        private static Guid IdFrom(object changeEvent)
        {
            return Reflector.ReadValueOrDefault(changeEvent, "Id", Guid.Empty);
        }

        protected T FromHistory<T>() where T : new()
        {
            var lastEventOfSearchedType = History.Concat(Changes).OfType<AggregateEventBag<T>>().LastOrDefault();
            return lastEventOfSearchedType == null
                ? default(T)
                : lastEventOfSearchedType.EventData;
        }
    }

}