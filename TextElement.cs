namespace WordProcessor
{
	public abstract class TextElement
	{
		protected TextElement()
		{

		}

		public static implicit operator TextElement (string value)
		{
			return new Run(value);
		}
	}
}