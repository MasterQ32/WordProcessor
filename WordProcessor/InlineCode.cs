using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordProcessor
{
	public sealed class InlineCode : TextElement
	{
		public InlineCode() : 
			this("")
		{
			
		}

		public InlineCode(string code)
		{
			this.Code = code;
		}

		public string Code { get; set; }
	}
}
