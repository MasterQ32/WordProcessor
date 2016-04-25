using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace WordProcessor
{
	public sealed class Document : IList<DocumentElement>
	{
		private readonly List<DocumentElement> elements = new List<DocumentElement>();

		private readonly NameValueCollection metaData = new NameValueCollection();

		public Document()
		{

		}

		public string Title { get; set; } = "Untitled";

		public NameValueCollection MetaData => this.metaData;

		#region IList<DocumentElement>

		public DocumentElement this[int index]
		{
			get
			{
				return elements[index];
			}

			set
			{
				elements[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return elements.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<DocumentElement>)elements).IsReadOnly;
			}
		}

		public CultureInfo Culture { get; set; } = CultureInfo.CurrentUICulture;

		public void Add(DocumentElement item)
		{
			elements.Add(item);
		}

		public void Clear()
		{
			elements.Clear();
		}

		public bool Contains(DocumentElement item)
		{
			return elements.Contains(item);
		}

		public void CopyTo(DocumentElement[] array, int arrayIndex)
		{
			elements.CopyTo(array, arrayIndex);
		}

		public IEnumerator<DocumentElement> GetEnumerator()
		{
			return ((IList<DocumentElement>)elements).GetEnumerator();
		}

		public int IndexOf(DocumentElement item)
		{
			return elements.IndexOf(item);
		}

		public void Insert(int index, DocumentElement item)
		{
			elements.Insert(index, item);
		}

		public bool Remove(DocumentElement item)
		{
			return elements.Remove(item);
		}

		public void RemoveAt(int index)
		{
			elements.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<DocumentElement>)elements).GetEnumerator();
		}

		#endregion
	}
}