namespace SeekYouRS.Tests.TestObjects
{
	class GenerischeKlasseMitEigenschaft<tEigenschaft>
	{
		public tEigenschaft Value { get; set; }

		public GenerischeKlasseMitEigenschaft(tEigenschaft value)
		{
			Value = value;
		}
	}
}