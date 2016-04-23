namespace WordProcessor
{
	public sealed class Formula : TextElement
	{
		public Formula() : 
			this("")
		{

		}

		public Formula(string value)
		{
			this.Code = value;
		}

		public string Code { get; set; }
	}
}