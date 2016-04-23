namespace WordProcessor
{
	public sealed class Link : TextWrapper
	{
		public Link()
		{

		}

		public Link(string target, params TextElement[] contents) :
			base(contents)
		{
			this.Target = target;
		}

		public string Target { get; set; }
	}
}