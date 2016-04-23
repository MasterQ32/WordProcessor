using System.IO;
using System.Text;

namespace WordProcessor
{
	public abstract class Parser
	{
		public Document Document { get; private set; }
		public TextReader Reader { get; private set; }

		protected abstract void Parse();

		public static Document Parse<T>(Stream stream, Encoding encoding)
			where T : Parser, new()
		{
			using (var reader = new StreamReader(stream, encoding))
			{
				return Parse<T>(reader);
			}
		}

		public static Document Parse<T>(TextReader reader)
			where T : Parser, new()
		{
			var parser = new T();
			parser.Document = new Document();
			parser.Reader = reader;
			parser.Parse();
			return parser.Document;
		}
	}
}