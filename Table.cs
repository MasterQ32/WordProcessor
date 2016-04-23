using System.Collections.Generic;

namespace WordProcessor
{
	public sealed class Table : DocumentElement
	{
		public Table()
		{

		}

		public TableRow Header { get; set; } = new TableRow();

		public List<TableRow> Rows { get; set; } = new List<TableRow>();
	}

	public sealed class TableRow : List<TableCell>
	{
		public TableRow() : base() { }

		public TableRow(IEnumerable<TableCell> cells) : base(cells) { }

		public TableRow(params TableCell[] cells) : base(cells) { }
	}

	public sealed class TableCell : TextWrapper
	{
		public TableCell() : 
			base()
		{

		}

		public TableCell(params TextElement[] contents) :
			base(contents)
		{

		}
	}
}