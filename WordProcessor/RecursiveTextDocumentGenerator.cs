using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordProcessor
{
	public abstract class RecursiveTextDocumentGenerator : TextDocumentGenerator
	{
		private readonly ObjectToStringConverter converter = new ObjectToStringConverter();
		
		protected string ToStringList(System.Collections.IEnumerable list) => string.Join("", list.Cast<object>().Select(ToString));

		protected string ToString(object obj) => converter.ToString(obj);

		public ObjectToStringConverter C => this.converter;

		protected override void Generate()
		{
			Writer.Write(converter.ToString(Document));
		}
	}
}
