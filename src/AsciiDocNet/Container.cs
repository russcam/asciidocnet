using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	/// <summary>
	///     An element that contains other elements
	/// </summary>
	public abstract class Container : IList<IElement> 
	{
		protected List<IElement> Elements;

		public int Count => Elements?.Count ?? 0;

		public bool IsReadOnly => false;

		public IElement this[int index]
		{
			get { return Elements?[index]; }
			set
			{
				if (Elements == null)
				{
					Elements = new List<IElement>();
				}

				Elements[index] = value;
				value.Parent = this;
			}
		}

		public static bool operator ==(Container left, Container right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Container left, Container right)
		{
			return !Equals(left, right);
		}

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

			return obj.GetType() == this.GetType() && Equals((Container)obj);
		}

		public override int GetHashCode()
		{
			return Elements.GetHashCode();
		}

		public void Add(IElement item)
		{
			if (Elements == null)
			{
				Elements = new List<IElement>();
			}

			Elements.Add(item);
			item.Parent = this;
		}

		public void Clear()
		{
			foreach (var element in Elements)
			{
				element.Parent = null;
			}

			Elements?.Clear();
		}

		public bool Contains(IElement item) => Elements != null && Elements.Contains(item);

		public void CopyTo(IElement[] array, int arrayIndex) => Elements?.CopyTo(array, arrayIndex);

		public IEnumerator<IElement> GetEnumerator()
		{
			return Elements?.GetEnumerator() ?? Enumerable.Empty<IElement>().GetEnumerator();
		}

		public int IndexOf(IElement item) => Elements?.IndexOf(item) ?? -1;

		public void Insert(int index, IElement item)
		{
			if (Elements == null)
			{
				Elements = new List<IElement>();
			}

			Elements.Insert(index, item);
			item.Parent = this;
		}

		public bool Remove(IElement item)
		{
			var remove = Elements != null && Elements.Remove(item);

			if (remove)
			{
				item.Parent = null;
			}

			return remove;
		}

		public void RemoveAt(int index)
		{
			if (Elements != null && Elements.Count > index)
			{
				var item = Elements[index];
				item.Parent = null;
				Elements.RemoveAt(index);
			}
		}

		protected bool Equals(Container other)
		{
			return Elements.Count == other.Elements.Count &&
			       Elements.SequenceEqual(other.Elements);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}