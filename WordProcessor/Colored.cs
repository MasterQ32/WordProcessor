using System.Drawing;

namespace WordProcessor
{
	public sealed class Colored : TextWrapper
	{
		public Color Background { get; set; }

		public Color Foreground { get; set; }

		public Colored()
		{

		}

		public Colored(Color background, Color foreground, params TextElement[] contents) :
			base(contents)
		{
			this.Foreground = foreground;
			this.Background = background;
		}

		public Colored(Color foreground, params TextElement[] contents) :
			this(Color.Transparent, foreground, contents)
		{

		}
	}
}