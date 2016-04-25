namespace WordProcessor
{
	public class FloatPanel : ElementWrapper
	{
		public FloatPanel(FloatAlign align, params DocumentElement[] contents) : 
			base(contents)
		{
			this.Align = align;
		}

		public FloatAlign Align { get; private set; }
	}

	public enum FloatAlign
	{
		Left,
		Right
	}
}