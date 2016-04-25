using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordProcessor.Parsers
{
	public sealed class Markdown : Parser
	{
		private string[] lines;
		private LineType[] lineTypes;
		private int cursor;

		protected override void Parse()
		{
			// Read all lines into the file. End-trims all lines
			this.lines = Reader
				.ReadToEnd()
				.Split(new[] { '\n' }, StringSplitOptions.None)
				.Select(s => s.TrimEnd())
				.SkipWhile(s => s.Length == 0)
				.ToArray();
			this.lineTypes = this.lines
				.Select((l, i) => DetermineLineType(i))
				.ToArray();
			this.cursor = 0;

			this.PostProcessLineTypes();

			while (this.cursor < this.lines.Length)
			{
				switch (this.lineTypes[this.cursor])
				{
					case LineType.Heading:
					{
						this.ReadHeading();
						break;
					}
					case LineType.Paragraph:
					{
						this.ReadParagraph();
						break;
					}
					case LineType.CodeBlock:
					{
						this.ReadCodeBlock();
						break;
					}
					case LineType.Image:
					{
						this.ReadImage();
						break;
					}
					case LineType.UnorderedList:
					{
						this.ReadList(ListType.Unordered);
						break;
					}
					case LineType.OrderedList:
					{
						this.ReadList(ListType.Ordered);
						break;
					}
					case LineType.TableHeader:
					{
						this.ReadTable();
						break;
					}
					case LineType.Blank:
					{
						// Ignore doubly blank lines
						this.cursor++;
						break;
					}
					default:
					{
						System.Diagnostics.Debug.WriteLine($"{lineTypes[cursor]} is not supported right now: {lines[cursor]}", "Markdown");
						// throw new NotSupportedException();
						this.cursor++;
						break;
					}
				}
			}
		}

		private TableRow ReadRow(string line)
		{
			if (line.StartsWith("|"))
				line = line.Substring(1);
			if (line.EndsWith("|"))
				line = line.Substring(0, line.Length - 1);
			return new TableRow(line
				.Split('|')
				.Select(s => s.Trim())
				.Select(s => CreateText(s))
				.Select(c => new TableCell(c)));
		}

		private void ReadTable()
		{
			var table = new Table();
			table.Header = ReadRow(lines[cursor++]);
			if (lineTypes[cursor++] != LineType.TableSeparator)
				throw new InvalidOperationException("Expected a table separator.");
			while(lineTypes[cursor] == LineType.TableRow)
			{
				table.Rows.Add(ReadRow(lines[cursor++]));
			}
			Document.Add(table);
		}

		private void ReadList(ListType type)
		{
			var currentIndent = 0;
			var stack = new Stack<ListItem>();
			var depths = new Dictionary<ListItem, int>();

			var root = new ListItem()
			{
				Text = "<ROOT>"
			};
			stack.Push(root);
			depths[root] = -1;

			LineType listLineType;
			switch (type)
			{
				case ListType.Ordered: listLineType = LineType.OrderedList; break;
				case ListType.Unordered: listLineType = LineType.UnorderedList; break;
				default: throw new NotSupportedException();
			}

			do
			{
				var line = lines[cursor];

				string text = "NOT SUPPORTED";
				switch (type)
				{
					case ListType.Unordered: text = line.Trim().Substring(1).Trim(); break;
					case ListType.Ordered:
					{
						var match = orderedListMatcher.Match(line);
						text = match.Groups[2].Value;
						break;
					}
				}

				var item = new ListItem() { Text = text };
				var indent = GetIndentation(line);

				depths[item] = indent;

				if (indent > currentIndent)
				{
					stack.Push(stack.Peek().Items.Last());
				}
				else if (indent < currentIndent)
				{
					while (depths[stack.Peek()] >= indent)
						stack.Pop();
				}

				stack.Peek().Items.Add(item);

				currentIndent = indent;

			} while (++this.cursor < this.lines.Length && lineTypes[cursor] == listLineType);

			Document.Add(new List(type, root.Items));

		}

		private void ReadImage()
		{
			var match = pictureMatcher.Match(lines[cursor++]);

			Document.Add(new Picture()
			{
				Location = match.Groups[2].Value,
				Target = match.Groups[2].Value,
				Description = match.Groups[1].Value,
			});
		}

		private void PostProcessLineTypes()
		{
			// Merge code blocks together
			for (int i = 0; i < this.lines.Length; i++)
			{
				switch (this.lineTypes[i])
				{
					// A blank will fill up code blocks together
					case LineType.Blank:
					{
						if (i <= 0)
							break;
						if (this.lineTypes[i - 1] == LineType.CodeBlock)
							this.lineTypes[i] = LineType.CodeBlock;
						break;
					}
					case LineType.Image:
					{
						if (pictureMatcher.IsMatch(lines[i]) == false)
							this.lineTypes[i] = LineType.Paragraph;
						break;
					}
					case LineType.CodeBlock:
					{
						if (i <= 0)
							break;
						if (lines[i].TrimStart().StartsWith("-") && lineTypes[i - 1] == LineType.UnorderedList)
							lineTypes[i] = LineType.UnorderedList;
						if (orderedListMatcher.IsMatch(lines[i]) && lineTypes[i - 1] == LineType.OrderedList)
							lineTypes[i] = LineType.OrderedList;
						break;
					}
					case LineType.MaybeTable:
					{
						var ptype = i > 0 ? lineTypes[i - 1] : LineType.Blank;
						var type = LineType.Paragraph;

						switch(ptype)
						{
							case LineType.Blank: type = LineType.TableHeader; break;
							case LineType.TableHeader: type = LineType.TableSeparator; break;
							case LineType.TableSeparator:
							case LineType.TableRow: type = LineType.TableRow; break;
						}
						
						lineTypes[i] = type;
						break;
					}
				}
			}
		}

		private void ReadHeading()
		{
			var line = this.lines[this.cursor++];
			var idx = line.IndexOf(" ");
			if (idx < 0)
				throw new InvalidOperationException("A heading requires a caption.");
			HeadingSize size;
			switch (idx)
			{
				case 1: size = HeadingSize.H1; break;
				case 2: size = HeadingSize.H2; break;
				case 3: size = HeadingSize.H3; break;
				case 4: size = HeadingSize.H4; break;
				default: throw new NotSupportedException($"A heading of size {idx} is not supported.");
			}
			this.Document.Add(new Heading(size, CreateText(line.Substring(idx + 1).Trim())));
		}

		private bool ProcessPattern(
			List<TextElement> elements,
			string text,
			Regex pattern,
			Func<Match, TextElement> eval)
		{
			var match = pattern.Match(text);
			if (match.Success == false)
				return false;
			if (match.Index > 0)
				elements.AddRange(CreateText(text.Substring(0, match.Index)));

			elements.Add(eval(match));

			if (match.Index + match.Length < text.Length)
				elements.AddRange(CreateText(text.Substring(match.Index + match.Length)));
			return true;
		}

		private class CreateTextOption
		{
			public Regex Pattern { get; set; }
			public Func<Match, TextElement> Evaluator { get; set; }
		}

		private static readonly Regex pictureMatcher = new Regex(
			@"!\[(.*?)\]\((.*?)\)",
			RegexOptions.Compiled);

		private static readonly Regex inlineCodeMatcher = new Regex(
			@"`(.*?)`",
			RegexOptions.Compiled);

		private static readonly Regex formulaMatcher = new Regex(
			@"\$\$(.*?)\$\$",
			RegexOptions.Compiled);

		private static readonly Regex urlMatcher = new Regex(
			@"\[(.*?)\]\((.*?)\)",
			RegexOptions.Compiled);

		private static readonly Regex boldMatcher = new Regex(
			@"\*\*(.*?)\*\*",
			RegexOptions.Compiled);

		private static readonly Regex italicMatcher = new Regex(
			@"\*(.*?)\*",
			RegexOptions.Compiled);

		private static readonly Regex orderedListMatcher = new Regex(
			@"^\s*(\d+)\s*\.\s*(.*)$",
			RegexOptions.Compiled);

		private TextElement[] CreateText(string text)
		{
			var results = new List<TextElement>();
			if (text.Length == 0)
				return new TextElement[0];
			var options = new[]
			{
				new CreateTextOption()
				{
					Pattern = inlineCodeMatcher,
					Evaluator = (match) => new InlineCode(match.Groups[1].Value)
				},
				new CreateTextOption()
				{
					Pattern = formulaMatcher,
					Evaluator = (match) => new Formula(match.Groups[1].Value),
				},
				new CreateTextOption()
				{
					Pattern = urlMatcher,
					Evaluator = (match) =>
					{
						var link = new Link();
						link.Target = match.Groups[2].Value;
						foreach (var item in CreateText(match.Groups[1].Value))
							link.Add(item);
						return link;
					},
				},
				new CreateTextOption()
				{
					Pattern = boldMatcher,
					Evaluator = (match) =>
					{
						var bold = new Bold();
						foreach (var item in CreateText(match.Groups[1].Value))
							bold.Add(item);
						return bold;
					},
				},
				new CreateTextOption()
				{
					Pattern = italicMatcher,
					Evaluator = (match) =>
					{
						var bold = new Italic();
						foreach (var item in CreateText(match.Groups[1].Value))
							bold.Add(item);
						return bold;
					},
				},
			};

			for (int i = 0; i < options.Length; i++)
			{
				if (ProcessPattern(results, text, options[i].Pattern, options[i].Evaluator))
					return results.ToArray();
			}
			return new[] { new Run(text) };
		}

		private void ReadParagraph()
		{
			var paragraph = new Paragraph();
			do
			{
				var items = CreateText(this.lines[this.cursor] + " ");
				for (int i = 0; i < items.Length; i++)
					paragraph.Add(items[i]);
			} while ((++this.cursor < this.lines.Length) && (this.lineTypes[this.cursor] == LineType.Paragraph));
			this.Document.Add(paragraph);
		}

		private void ReadCodeBlock()
		{
			var code = new List<string>();
			do
			{
				code.Add(lines[cursor]);
			} while ((++this.cursor < this.lines.Length) && (this.lineTypes[this.cursor] == LineType.CodeBlock));

			int indent = code.Where(l => l.Length > 0).Min(GetIndentation);
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].Length > 0)
					code[i] = code[i].Substring(indent);
			}
			this.Document.Add(new CodeBlock(code.ToArray()));
		}

		private static int GetIndentation(string str)
		{
			return str.TakeWhile(c => char.IsWhiteSpace(c) || (c == '\t')).Count();
		}

		private LineType DetermineLineType(int i)
		{
			var line = lines[i];
			if (line.Length == 0)
				return LineType.Blank;
			if (line.StartsWith("#"))
				return LineType.Heading;
			if (line.StartsWith("\t") || line.StartsWith("  "))
				return LineType.CodeBlock;
			if (line.StartsWith("!["))
				return LineType.Image;
			if (line.Contains("|"))
				return LineType.MaybeTable;
			if (line.StartsWith("-"))
				return LineType.UnorderedList;
			if (orderedListMatcher.IsMatch(line))
				return LineType.OrderedList;
			return LineType.Paragraph;
		}

		enum LineType
		{
			Paragraph,
			MaybeTable,
			TableHeader,
			TableSeparator,
			TableRow,
			CodeBlock,
			Heading,
			Blank,
			UnorderedList,
			OrderedList,
			Image,
		}
	}
}