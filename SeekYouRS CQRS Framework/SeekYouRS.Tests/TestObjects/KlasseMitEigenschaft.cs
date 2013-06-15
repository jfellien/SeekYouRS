using System;

namespace SeekYouRS.Tests.TestObjects
{
	class KlasseMitEigenschaft
	{
		public Guid Id { get; set; }

		public KlasseMitEigenschaft(Guid id)
		{
			Id = id;
		}
	}
}