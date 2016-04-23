using System.Collections.Generic;

namespace WordProcessor
{
	public sealed class Paragraph : TextContent
	{
		public Paragraph()
		{

		}

		public Paragraph(params TextElement[] contents) : 
			base(contents)
		{
			
		}

		public TextAlign Alignment { get; set; } = TextAlign.Unspecified;
	}

	public enum TextAlign
	{
		Unspecified,
		Left,
		Center,
		Right,
		Block
	}
}