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
		/// Gets the id of the current Aggregeate. Derived class should implements the method to read Id from history.
		/// </summary>
		public abstract Guid Id { get; }

		/// <summary>
		/// Gets the list of current changes. List will reset after successful saving.
		/// </summary>
		public IList<IAmAnAggregateEvent> Changes{ get; private set; }
		/// <summary>
		/// Gets or sets the list of all status changes
		/// </summary>
		public IEnumerable<IAmAnAggregateEvent> History { get; set; }

		/// <summary>
		/// Puts the change event into list of changes. The list will use by saveing the Aggregate.
		/// </summary>
		/// <typeparam name="T">Type of Event</typeparam>
		/// <param name="changeEvent">Event with change parameters</param>
		protected void ApplyChanges<T>(T changeEvent) where T : IAmAnAggregateEvent
		{
			Changes.Add(changeEvent);
		}
		/// <summary>
		/// Gets all Events of specific type from the list of all historical AggregateEvents
		/// </summary>
		/// <typeparam name="T">Type of Event</typeparam>
		/// <returns>Data of history</returns>
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