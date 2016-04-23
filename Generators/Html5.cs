using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WordProcessor.Generators
{
	public sealed class Html5 : TextDocumentGenerator
	{
		protected override void Generate()
		{
			Writer.WriteLine("<!DOCTYPE html>");
			Writer.WriteLine("<html>");
			Writer.WriteLine("<head>");

			Writer.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\">", Writer.Encoding.WebName);
			Writer.WriteLine("<title>{0}</title>", this.Document.Title);

			if (this.Document.MetaData["stylesheet"] != null)
			{
				Writer.WriteLine(
					"<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" charset=\"{1}\">",
					this.Document.MetaData["stylesheet"],
					this.Document.MetaData["stylesheet-enc"] ?? "utf-8");
			}

			Writer.WriteLine("</head>");
			Writer.WriteLine("<body lang=\"{0}\">", this.Document.Culture.Name);

			WriteContents(this.Document);

			Writer.WriteLine("</body>");
			Writer.WriteLine("</html>");
		}

		private void WriteContents(IEnumerable<DocumentElement> elements)
		{
			foreach (var element in elements)
			{
				WriteElement(element);
			}
		}

		private void WriteElement(DocumentElement element)
		{
			WriteElement<Heading>(element, (h) =>
			{
				string htype;
				switch (h.Size)
				{
					case HeadingSize.H1: htype = "h1"; break;
					case HeadingSize.H2: htype = "h2"; break;
					case HeadingSize.H3: htype = "h3"; break;
					case HeadingSize.H4: htype = "h4"; break;
					default: throw new NotSupportedException($"{h.Size} is not a supported heading size.");
				}
				Writer.Write("<{0}>", htype);
				WriteTextElements(h);
				Writer.WriteLine("</{0}>", htype);
			});
			WriteElement<Paragraph>(element, (p) =>
			{
				string alignment;
				switch (p.Alignment)
				{
					case TextAlign.Left: alignment = "left"; break;
					case TextAlign.Right: alignment = "right"; break;
					case TextAlign.Block: alignment = "justify"; break;
					case TextAlign.Center: alignment = "center"; break;
					case TextAlign.Unspecified: alignment = "inherit"; break;
					default: throw new NotSupportedException($"{p.Alignment} is not a supported text alignment.");
				}
				Writer.Write("<p style=\"text-align: {0}\">", alignment);
				WriteTextElements(p);
				Writer.WriteLine("</p>");
			});
			WriteElement<Picture>(element, (p) =>
			{
				if (p.Target != null)
					Writer.Write("<a href=\"{0}\">", p.Target);
				Writer.Write("<img src=\"{0}\" />", p.Location);
				if (p.Target != null)
					Writer.Write("</a>");
				Writer.WriteLine(); 
			});
			WriteElement<FloatPanel>(element, (f) =>
			{
				string align;
				switch (f.Align)
				{
					case FloatAlign.Left: align = "left"; break;
					case FloatAlign.Right: align = "right"; break;
					default: throw new NotSupportedException($"{f.Align} is not a supported floating alignment.");
				}
				Writer.Write("<div class=\"float\" style=\"float: {0}\">", align);
				WriteContents(f);
				Writer.Write("</div>");
			});
			WriteElement<CodeBlock>(element, (c) =>
			{
				Writer.Write("<code><pre>");
				foreach(var line in c.Lines)
				{
					Writer.WriteLine(line);
				}
				Writer.WriteLine("</pre></code>");
			});
			WriteElement<List>(element, WriteList);
			WriteElement<Table>(element, (t) =>
			{
				Writer.WriteLine("<table>");
				Writer.WriteLine("<tr>");
				foreach (var cell in t.Header)
				{
					Writer.Write("<th>");
					WriteText(cell);
					Writer.WriteLine("</th>");
				}
				Writer.WriteLine("</tr>");
				foreach(var row in t.Rows)
				{
					Writer.WriteLine("<tr>");
					foreach(var cell in row)
					{
						Writer.Write("<td>");
						WriteText(cell);
						Writer.WriteLine("</td>");
					}
					Writer.WriteLine("</tr>");
				}

				Writer.WriteLine("</table>");
			});
		}

		private void WriteList(List list)
		{
			string tag;
			switch(list.Type)
			{
				case ListType.Unordered: tag = "ul"; break;
				case ListType.Ordered: tag = "ol"; break;
				default: throw new NotSupportedException($"{list.Type} is not a supported list type.");
			}
			WriteList(tag, list.Items);
		}

		private void WriteList(string listTag, IEnumerable<ListItem> items)
		{
			Writer.WriteLine("<{0}>", listTag);
			foreach (var item in items)
			{
				Writer.Write("<li>{0}", item.Text);
				if(item.Items.Count > 0)
				{
					WriteList(listTag, item.Items);
				}
				Writer.WriteLine("</li>");
			}
			Writer.WriteLine("</{0}>", listTag);
		}

		private void WriteElement<T>(DocumentElement element, Action<T> Writer)
			where T : DocumentElement
		{
			var e = element as T;
			if (e != null)
				Writer(e);
		}

		private void WriteTextElements(IEnumerable<TextElement> elements)
		{
			foreach (var item in elements)
				WriteText(item);
		}

		private void WriteText(TextElement element)
		{
			WriteText<Run>(element, WriteRun);
			WriteText<Bold>(element, WriteBold);
			WriteText<Italic>(element, WriteItalic);
			WriteText<Colored>(element, WriteColored);
			WriteText<Link>(element, WriteLink);
			WriteText<InlineCode>(element, WriteInlineCode);
			WriteText<TableCell>(element, (c) => WriteTextElements(c));
		}

		private void WriteInlineCode(InlineCode obj)
		{
			Writer.Write("<code class=\"inline\">{0}</code>", obj.Code);
		}

		private void WriteRun(Run run)
		{
			Writer.Write(run.Text);
		}

		private void WriteLink(Link link)
		{
			Writer.Write("<a href=\"{0}\">", link.Target);
			WriteTextElements(link);
			Writer.Write("</a>");
		}

		private void WriteBold(Bold bold)
		{
			Writer.Write("<b>");
			WriteTextElements(bold);
			Writer.Write("</b>");
		}

		private void WriteItalic(Italic italic)
		{
			Writer.Write("<i>");
			WriteTextElements(italic);
			Writer.Write("</i>");
		}

		private static string HexColor(Color color)
		{
			if (color.IsNamedColor)
				return color.Name;
			return "#" +
				Convert.ToString(color.R, 16).PadLeft(2, '0') +
				Convert.ToString(color.G, 16).PadLeft(2, '0') +
				Convert.ToString(color.B, 16).PadLeft(2, '0') +
				Convert.ToString(color.A, 16).PadLeft(2, '0');
		}

		private void WriteColored(Colored colored)
		{
			var foreground = "";
			var background = "";
			if (colored.Background != Color.Transparent)
				background = "background-color: " + HexColor(colored.Background) + ";";
			if (colored.Foreground != Color.Transparent)
				foreground = "color: " + HexColor(colored.Foreground) + ";";

			Writer.Write("<span style=\"{0} {1}\">", foreground, background);
			WriteTextElements(colored);
			Writer.Write("</span>");
		}

		private void WriteText<T>(TextElement element, Action<T> processor)
			where T : TextElement
		{
			var e = element as T;
			if (e != null)
				processor(e);
		}
	}
}