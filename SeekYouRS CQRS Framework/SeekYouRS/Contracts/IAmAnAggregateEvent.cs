using System;

namespace SeekYouRS.Contracts
{
	/// <summary>
	/// Describes a derived class as an AggregateEvent
	/// </summary>
	public interface IAmAnAggregateEvent
	{
		/// <summary>
		/// Gets or sets the Id of the Aggregate who assigned this AggregateEvent
		/// </summary>
		Guid Id { get; set; }
	}
}