using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	/// <summary>
	///     An element that contains inline elements
	/// </summary>
	public abstract class InlineContainer : IList<IInlineElement>
	{
		protected List<IInlineElement> Elements;

		public abstract InlineElementType ContainElementType { get; }

		public int Count => Elements?.Count ?? 0;

		public bool IsReadOnly => false;

		public IInlineElement this[int index]
		{
			get { return Elements?[index]; }
			set
			{
				if (Elements == null)
				{
					Elements = new List<IInlineElement>();
				}

				Elements[index] = value;
			}
		}

		public static bool operator ==(InlineContainer left, InlineContainer right) => Equals(left, right);

		public static bool operator !=(InlineContainer left, InlineContainer right) => !Equals(left, right);

		public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return obj.GetType() == this.GetType() && Equals((InlineContainer)obj);
		}

		public override int GetHashCode() => Elements.GetHashCode();

		public void Add(IInlineElement item)
		{
			if (Elements == null)
			{
				Elements = new List<IInlineElement>();
			}

			Elements.Add(item);
		}

		public void Clear() => Elements?.Clear();

		public bool Contains(IInlineElement item) => Elements != null && Elements.Contains(item);

		public void CopyTo(IInlineElement[] array, int arrayIndex) => Elements?.CopyTo(array, arrayIndex);

		public IEnumerator<IInlineElement> GetEnumerator() => 
			Elements?.GetEnumerator() ?? Enumerable.Empty<IInlineElement>().GetEnumerator();

		public int IndexOf(IInlineElement item) => Elements?.IndexOf(item) ?? -1;

		public void Insert(int index, IInlineElement item)
		{
			if (Elements == null)
			{
				Elements = new List<IInlineElement>();
			}

			Elements.Insert(index, item);
		}

		public bool Remove(IInlineElement item) => Elements != null && Elements.Remove(item);

		public void RemoveAt(int index) => Elements?.RemoveAt(index);

		protected bool Equals(InlineContainer other) => 
			Elements.Count == other.Elements.Count && Elements.SequenceEqual(other.Elements);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}