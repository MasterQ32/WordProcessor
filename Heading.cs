namespace WordProcessor
{
	public sealed class Heading : TextContent
	{
		public Heading(params TextElement[] contents) : 
			this(HeadingSize.H1, contents)
		{

		}

		public Heading(HeadingSize size, params TextElement[] contents) :
			base(contents)
		{
			this.Size = size;
		}

		public HeadingSize Size { get; set; }
	}

	public enum HeadingSize
	{
		H1,
		H2,
		H3,
		H4,
	}
}