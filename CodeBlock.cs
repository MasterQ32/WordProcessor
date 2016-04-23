namespace WordProcessor
{
	public sealed class CodeBlock : DocumentElement
	{
		public CodeBlock(string[] lines)
		{
			this.Lines = (string[])lines.Clone();
		}

		public string[] Lines { get;  set; }
	}
}