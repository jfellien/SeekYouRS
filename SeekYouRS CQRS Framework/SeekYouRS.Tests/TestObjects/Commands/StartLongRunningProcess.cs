using System;

namespace SeekYouRS.Tests.TestObjects.Commands
{
	internal class StartLongRunningProcess 
	{
		public Guid Id { get; set; }

		public int Milliseconds { get; set; }
	}
}