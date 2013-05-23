using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.EventSource;

namespace SeekYouRS
{
    public abstract class Aggegate
    {
        protected Aggegate()
        {
            Changes = new List<Event>();
        }

        public Guid Id { get; set; }

        public IList<Event> Changes{ get; private set; }

        public IEnumerable<Event> History { get; set; }

        protected void ApplyChanges<T>(T changeEvent) where T : class
        {
            if(Id == Guid.Empty)
                throw new ArgumentException("Id of an Aggregate can not be Empty");

            Changes.Add(new EventBag<T>(Id){EventData = changeEvent});
        }

        protected T FromHistory<T>() where T : new ()
        {
            var lastEventOfSearchedType = History.Concat(Changes).OfType<EventBag<T>>().LastOrDefault();
            return lastEventOfSearchedType == null 
                ? new T() 
                : lastEventOfSearchedType.EventData;
        }
    }
}