using System.Collections;
using System.Collections.Generic;

namespace WordProcessor
{
	public abstract class TextWrapper : TextElement, IList<TextElement>
	{
		private readonly List<TextElement> elements = new List<TextElement>();

		protected TextWrapper()
		{

		}

		protected TextWrapper(params TextElement[] contents)
		{
			this.elements = new List<TextElement>(contents);
		}

		#region IList<TextElement>

		public TextElement this[int index]
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
				return ((IList<TextElement>)elements).IsReadOnly;
			}
		}

		public void Add(TextElement item)
		{
			elements.Add(item);
		}

		public void Clear()
		{
			elements.Clear();
		}

		public bool Contains(TextElement item)
		{
			return elements.Contains(item);
		}

		public void CopyTo(TextElement[] array, int arrayIndex)
		{
			elements.CopyTo(array, arrayIndex);
		}

		public IEnumerator<TextElement> GetEnumerator()
		{
			return ((IList<TextElement>)elements).GetEnumerator();
		}

		public int IndexOf(TextElement item)
		{
			return elements.IndexOf(item);
		}

		public void Insert(int index, TextElement item)
		{
			elements.Insert(index, item);
		}

		public bool Remove(TextElement item)
		{
			return elements.Remove(item);
		}

		public void RemoveAt(int index)
		{
			elements.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<TextElement>)elements).GetEnumerator();
		}

		#endregion

	}
}