using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Storing;

using SetId = System.Action<System.Guid>;

namespace SeekYouRS
{
    public abstract class Aggregate
    {
        protected Aggregate()
        {
            Changes = new List<AggregateEvent>();
        }

        public abstract Guid Id { get; }

        public IList<AggregateEvent> Changes{ get; private set; }

        public IEnumerable<AggregateEvent> History { get; set; }

        protected void ApplyChanges<T>(T changeEvent) where T : class
        {
            var accessors = IdFrom(changeEvent);

            var id = accessors.Item1;
            var setId = accessors.Item2;

            if (id == Guid.Empty)
            {
                id = Id;
                setId(id);
                // NOTE: needs Tests and error handling
            }

            Changes.Add(new AggregateEventBag<T>(id){EventData = changeEvent});
        }

        private static Tuple<Guid, SetId> IdFrom(object changeEvent)
        {
            var propertyInfos = changeEvent.GetType().GetProperties();

            try
            {
                var identifier = propertyInfos.SingleOrDefault(pi => 
                    pi.Name.Equals("id", StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return Tuple.Create<Guid,SetId>(Guid.Empty, id => {});
                var objId = (Guid) identifier.GetValue(changeEvent);
                Action<Guid> set = id => identifier.SetValue(changeEvent, id);
                return Tuple.Create(objId, set);
            }
            catch (Exception)
            {
                return Tuple.Create<Guid,SetId>(Guid.Empty, id => {});
            }
        }

        protected T FromHistory<T>() where T : new ()
        {
            var lastEventOfSearchedType = History.Concat(Changes).OfType<AggregateEventBag<T>>().LastOrDefault();
            return lastEventOfSearchedType == null 
                ? default(T) 
                : lastEventOfSearchedType.EventData;
        }
    }
}