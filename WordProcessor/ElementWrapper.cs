using System.Collections;
using System.Collections.Generic;

namespace WordProcessor
{
	public abstract class ElementWrapper : DocumentElement, IList<DocumentElement>
	{
		private readonly List<DocumentElement> contents;

		protected ElementWrapper(params DocumentElement[] contents)
		{
			this.contents = new List<DocumentElement>(contents);
		}

		#region IList<DocumentElement>

		public DocumentElement this[int index]
		{
			get
			{
				return contents[index];
			}

			set
			{
				contents[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return contents.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<DocumentElement>)contents).IsReadOnly;
			}
		}

		public void Add(DocumentElement item)
		{
			contents.Add(item);
		}

		public void Clear()
		{
			contents.Clear();
		}

		public bool Contains(DocumentElement item)
		{
			return contents.Contains(item);
		}

		public void CopyTo(DocumentElement[] array, int arrayIndex)
		{
			contents.CopyTo(array, arrayIndex);
		}

		public IEnumerator<DocumentElement> GetEnumerator()
		{
			return ((IList<DocumentElement>)contents).GetEnumerator();
		}

		public int IndexOf(DocumentElement item)
		{
			return contents.IndexOf(item);
		}

		public void Insert(int index, DocumentElement item)
		{
			contents.Insert(index, item);
		}

		public bool Remove(DocumentElement item)
		{
			return contents.Remove(item);
		}

		public void RemoveAt(int index)
		{
			contents.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<DocumentElement>)contents).GetEnumerator();
		}

		#endregion
	}
}