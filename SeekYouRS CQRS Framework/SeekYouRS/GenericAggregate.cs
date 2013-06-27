using System;
using System.Collections.Generic;
using SeekYouRS.Utilities;

using Key = System.Type;

namespace SeekYouRS
{
    /// <summary>
    /// Base class of an Aggregate. 
    /// Aggregates collects Entities and Value Objects of a specific domain context. 
    /// An Aggreagte should be private in Domain scope.
    /// </summary>
    public abstract class GenericAggregate<T> : Aggregate
    {
        private readonly Dictionary<Type, Func<Option<T>, AggregateEvent, Option<T>>> _modelTransformers;

        public GenericAggregate()
            : base()
        {
            _modelTransformers = new Dictionary<Key, Func<Option<T>, AggregateEvent, Option<T>>>();
        }

        private static Key KeyOf<TData>()
        {
            return typeof (AggregateEventBag<TData>);
        }

        protected void RegisterModelTransformer<TData>(Func<Option<T>, TData, Option<T>> modelTransform)
        {
            Func<Option<T>, AggregateEvent, Option<T>> transformer =
                (model, ae) =>
                {
                    var bag = ae as AggregateEventBag<TData>;
                    return bag == null ? model : modelTransform(model, bag.EventData);
                };

            _modelTransformers.Add(KeyOf<TData>(), transformer);
        }

        protected Option<T> AggregateCurrentModel()
        {
            return base.AggregateHistoryAndChanges(Option<T>.None, AggregateEventFold);
        }

        protected Option<T> AggregateEventFold(Option<T> state, AggregateEvent ev)
        {
            return
                Option.Try<Key, Func<Option<T>, AggregateEvent, Option<T>>>(_modelTransformers.TryGetValue, ev.GetType())
                      .ApplyTo(state, ev)
                      .DefaultsTo(state);
        }
    }
}