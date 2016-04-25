using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WordProcessor.Generators
{
	public sealed class Latex : RecursiveTextDocumentGenerator
	{
		public Latex()
		{
			C.Register<Document>(d =>
			{
				return @"\documentclass[a4paper, 12pt]{article}
\usepackage[utf8]{inputenc}
\usepackage[ngerman]{babel}
% \usepackage{listings}
\usepackage{hyperref}
\usepackage{graphicx}

\begin{document}
" +
			string.Join("\n\n", d.Select(ToString)) + @"
\end{document}";
			});

			C.Register<Heading>(h =>
			{
				switch (h.Size)
				{
					case HeadingSize.H1: return @"\section{" + ToStringList(h) + "}";
					case HeadingSize.H2: return @"\subsection{" + ToStringList(h) + "}";
					case HeadingSize.H3: return @"\subsubsection{" + ToStringList(h) + "}";
					case HeadingSize.H4: return @"\paragraph{" + ToStringList(h) + "}";
					default: throw new NotSupportedException();
				}
			});

			C.Register<Paragraph>(p => ToStringList(p));

			C.Register<Run>(r => r.Text);

			C.Register<Formula>(f => "$ " + f.Code + " $");

			C.Register<InlineCode>(f => @"\texttt{" + f.Code + "}");

			C.Register<Link>(l => @"\href{" + l.Target + "}{" + ToStringList(l) + "}");

			C.Register<Bold>(b => @"\textbf{" + ToStringList(b) + "}");

			C.Register<Italic>(b => @"\textit{" + ToStringList(b) + "}");

			C.Register<Picture>(p => @"\includegraphics[width=\linewidth]{" + Path.GetFileNameWithoutExtension(p.Location) + "}");

			C.Register<CodeBlock>(c => @"\begin{verbatim}" + "\n" + string.Join("\n", c.Lines) + "\n" + @"\end{verbatim}");

			C.Register<Table>(t =>
			{
				int size = Math.Max(t.Header.Count, t.Rows.Max(r => r.Count));

				var result = @"\begin{tabular}{" + new string('l', size) + "}\n";

				result += ToString(t.Header);
				result += "\\hline\n";
				result += string.Join("\n", t.Rows.Select(ToString));

				result += "\n" + @"\end{tabular}";
				return result;
			});

			C.Register<TableRow>(r => string.Join(" & ", r.Select(ToString)) + @"\\" + "\n");

			C.Register<TableCell>(c => ToStringList(c));

			C.Register<List>(l =>
			{
				var result = "";
				if (l.Type == ListType.Ordered)
					result += @"\begin{enumerate}" + "\n";
				else
					result += @"\begin{itemize}" + "\n";

				foreach(var item in l.Items)
				{
					result += ToString(new ListHelper()
					{
						Type = l.Type,
						Item = item,
					});
				}					

				if (l.Type == ListType.Ordered)
					result += @"\end{enumerate}";
				else
					result += @"\end{itemize}";
				return result;
			});

			C.Register<ListHelper>(h =>
			{
				string result = "";
				result += @"\item " + h.Item.Text;
				if (h.Item.Items.Count > 0)
				{
					result += "\n" + ToString(new List(h.Type, h.Item.Items));
				}
				result += "\n";
				return result;
			});
		}

		private class ListHelper
		{
			public ListType Type { get; set; }

			public ListItem Item { get; set; }
		}
	}
}
