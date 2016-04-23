namespace WordProcessor
{
	public sealed class Picture : DocumentElement
	{
		public Picture()
		{

		}

		public Picture(string location)
		{
			this.Location = location;
		}

		public string Description { get; set; }

		public string Location { get; set; }

		public string Target { get; set; }
	}
}