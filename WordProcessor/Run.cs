namespace WordProcessor
{
	public sealed class Run : TextElement
	{
		public Run() : 
			this("")
		{

		}

		public Run(string value)
		{
			this.Text = value;
		}

		public string Text { get; set; }
	}
}