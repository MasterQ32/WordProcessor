using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordProcessor
{
	class Program
	{
		static void Main(string[] args)
		{
			Document doc;
			using (var fs = File.Open("Samples/markdown.md", FileMode.Open, FileAccess.Read))
			{
				doc = Parser.Parse<Parsers.Markdown>(fs, Encoding.UTF8);
			}
			
			// doc.MetaData["stylesheet"] = "style.css";
			// doc.MetaData["stylesheet-enc"] = "something-wicked";
			
			using (var fs = File.Open("result.htm", FileMode.Create, FileAccess.Write))
			{
				DocumentGenerator.Generate<Generators.Html5>(fs, doc);
			}
			using (var fs = File.Open("result.md", FileMode.Create, FileAccess.Write))
			{
				DocumentGenerator.Generate<Generators.Markdown>(fs, doc);
			}
		}
	}
}
