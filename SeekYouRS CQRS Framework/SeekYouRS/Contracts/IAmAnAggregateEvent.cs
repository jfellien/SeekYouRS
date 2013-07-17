using System;

namespace SeekYouRS.Contracts
{
	public interface IAmAnAggregateEvent
	{
		Guid Id { get; set; }
	}
}