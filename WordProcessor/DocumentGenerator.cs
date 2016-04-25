using System;
using System.IO;
using System.Text;

namespace WordProcessor
{
	public abstract class TextDocumentGenerator : DocumentGenerator
	{
		private TextWriter writer;

		public override void Generate(FileStream fs, Encoding encoding)
		{
			using (this.writer = new StreamWriter(fs, encoding))
			{
				this.Generate();
			}
		}

		protected abstract void Generate();

		protected TextWriter Writer => this.writer;
	}

	public abstract class DocumentGenerator
	{
		public Document Document { get; private set; }

		public abstract void Generate(FileStream fs, Encoding encoding);

		public static void Generate<T>(FileStream fs, Document doc)
			where T : DocumentGenerator, new()
		{
			Generate<T>(fs, Encoding.UTF8, doc);
		}

		public static void Generate<T>(FileStream fs, Encoding encoding, Document doc)
			where T : DocumentGenerator, new ()
		{
			var generator = new T();
			generator.Document = doc;
			generator.Generate(fs, encoding);
		}
	}
}