namespace WordProcessor
{
	public sealed class Bold : TextWrapper
	{
		public Bold()
		{

		}

		public Bold(params TextElement[] contents) : 
			base(contents)
		{

		}
	}
}