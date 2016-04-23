using System.Collections.Generic;

namespace WordProcessor
{
	public sealed class List : DocumentElement
	{
		public List() : 
			this(ListType.Unordered)
		{

		}

		public List(ListType type)
		{
			this.Type = type;
			this.Items = new List<ListItem>();
		}

		public List(ListType type, List<ListItem> items) : 
			this(type)
		{
			this.Items = new List<ListItem>(items);
		}

		public ListType Type { get; set; }

		public List<ListItem> Items { get; set; }
	}

	public enum ListType
	{
		Unordered,
		Ordered,
	}

	public sealed class ListItem
	{
		public ListItem() : 
			this("")
		{

		}

		public ListItem(string text, params ListItem[] items)
		{
			this.Text = text;
			this.Items.AddRange(items);
		}

		public string Text { get; set; }

		public List<ListItem> Items { get; } = new List<ListItem>();

		public override string ToString() => $"{this.Text} [{this.Items.Count}]";
    }
}