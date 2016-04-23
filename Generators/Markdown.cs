using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordProcessor.Generators
{
	public sealed class Markdown : TextDocumentGenerator
	{
		private readonly ObjectToStringConverter converter = new ObjectToStringConverter();

		public Markdown()
		{
			var C = converter;

			C.Register<Document>(d =>
			{
				return string.Join("\n\n", d.Select(ToString));
			});

			C.Register<Heading>(h =>
			{
				switch (h.Size)
				{
					case HeadingSize.H1: return "# " + ToStringList(h);
					case HeadingSize.H2: return "## " + ToStringList(h);
					case HeadingSize.H3: return "### " + ToStringList(h);
					case HeadingSize.H4: return "#### " + ToStringList(h);
					default: throw new NotSupportedException();
				}
			});

			C.Register<Run>(r => r.Text);

			C.Register<Paragraph>(p => ToStringList(p));

			C.Register<Bold>(b => "**" + ToStringList(b) + "**");

			C.Register<Italic>(i => "*" + ToStringList(i) + "*");

			C.Register<InlineCode>(c => "`" + c.Code + "`");

			C.Register<Link>(l => "[" + ToStringList(l) + "](" + l.Target + ")");

			C.Register<Picture>(p => "![" + p.Description + "](" + p.Location + ")");

			C.Register<CodeBlock>(c => string.Join("\n", c.Lines.Select(l => "\t" + l)));

			C.Register<Table>(t =>
			{
				return ToString(t.Header) + string.Join("-|-", t.Header.Select(x => "-")) + "\n" + ToStringList(t.Rows);
			});
			C.Register<TableRow>(r => string.Join(" | ", r.Select(c => ToString(c))) + "\n");
			C.Register<TableCell>(c => ToStringList(c));

			C.Register<List>(l => string.Join("\n", l.Items.Select((i, _) => new Markdown.ListHelper() { Index = _, Item = i, Indent = 0, Type = l.Type, }).Select(ToString)));

			C.Register<Markdown.ListHelper>(h =>
			{
				string prefix = "- ";
				if (h.Type == ListType.Ordered)
					prefix = $"{h.Index + 1}. ";

				var result = new string('\t', h.Indent) + prefix + h.Item.Text;
				if (h.Item.Items.Count > 0)
				{
					result += "\n";
					result += string.Join(
						"\n",
						h.Item.Items.Select((i, _) => new Markdown.ListHelper()
						{
							Index = _,
							Item = i,
							Indent = h.Indent + 1,
							Type = h.Type
						})
						.Select(ToString));
				}
				return result;
			});
		}

		private class ListHelper
		{
			public int Indent { get; set; } = 0;

			public int Index { get; set; } = 0;

			public ListType Type { get; set; } = ListType.Unordered;

			public ListItem Item { get; set; } = null;
		}

		private string ToStringList(System.Collections.IEnumerable list) => string.Join("", list.Cast<object>().Select(ToString));

		private string ToString(object obj) => converter.ToString(obj);

		protected override void Generate()
		{
			Writer.Write(converter.ToString(Document));
		}
	}
}
