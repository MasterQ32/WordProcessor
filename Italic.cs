namespace WordProcessor
{
	public sealed class Italic : TextWrapper
	{
		public Italic()
		{

		}

		public Italic(params TextElement[] contents) :
			base(contents)
		{

		}
	}
}