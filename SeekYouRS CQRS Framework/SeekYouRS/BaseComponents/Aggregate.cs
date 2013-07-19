using System;
using System.Collections.Generic;
using System.Linq;
using SeekYouRS.Contracts;

namespace SeekYouRS.BaseComponents
{
	/// <summary>
	/// Base class of an Aggregate. 
	/// Aggregates collects Entities and Value Objects of a specific domain context. 
	/// An Aggreagte should be private in Domain scope.
	/// </summary>
	public abstract class Aggregate
	{
		protected Aggregate()
		{
			Changes = new List<IAmAnAggregateEvent>();
		}

		/// <summary>
		/// Gets the id of the current Aggregeate. Derived class should implements the method to read Id from History.
		/// </summary>
		protected abstract Guid Id { get; }

		/// <summary>
		/// Gets the list of current changes. List will reset after successful saving.
		/// </summary>
		internal IList<IAmAnAggregateEvent> Changes{ get; private set; }
		/// <summary>
		/// Gets or sets the list of all status changes.
		/// </summary>
		internal IEnumerable<IAmAnAggregateEvent> History { get; set; }

		/// <summary>
		/// Adds the change event into list of changes.
		/// </summary>
		/// <typeparam name="T">Type of Event</typeparam>
		/// <param name="changeEvent">Event with change parameters</param>
		protected void ApplyChanges<T>(T changeEvent) where T : IAmAnAggregateEvent
		{
			Changes.Add(changeEvent);
		}
		/// <summary>
		/// Gets the last AggregateEvent of specific type from list of all historical Events
		/// </summary>
		/// <typeparam name="T">Type of Event</typeparam>
		/// <returns>Last AggregateEvent of Type T</returns>
		protected T FromHistory<T>() where T : new()
		{
			var allEvents = History.Concat(Changes);

			var lastEventOfSearchedType = allEvents
				.OfType<T>()
				.LastOrDefault();

			return lastEventOfSearchedType == null
				? default(T)
				: lastEventOfSearchedType;
		}
	}
}