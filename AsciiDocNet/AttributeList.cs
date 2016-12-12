using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class AttributeList : IList<Attribute>
	{
		private List<Attribute> _attributes;

		public Anchor Anchor { get; set; }

		public int Count => _attributes?.Count ?? 0;

		public bool HasAnchor => Anchor != null;

		public bool HasTitle => Title != null;

		public bool IsReadOnly => false;

		public Title Title { get; set; }

		public Attribute this[int index]
		{
			get { return _attributes?[index]; }
			set
			{
				if (_attributes == null)
				{
					_attributes = new List<Attribute>();
				}

				_attributes[index] = value;
			}
		}

		public Attribute this[string name]
		{
			get
			{
				return _attributes?.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			}
			set
			{
				_attributes?.RemoveAll(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
				if (_attributes == null)
				{
					_attributes = new List<Attribute>();
				}

				_attributes.Add(value);
			}
		}

		public static bool operator ==(AttributeList left, AttributeList right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(AttributeList left, AttributeList right)
		{
			return !Equals(left, right);
		}

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
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((AttributeList)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = _attributes?.GetHashCode() ?? 0;
				hashCode = (hashCode * 397) ^ (Anchor?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		public AttributeList Add(AttributeList attributeList)
		{
			if (attributeList == null)
			{
				return this;
			}

			Add(attributeList.Title);
			Add(attributeList.Anchor);

			if (_attributes == null)
			{
				_attributes = new List<Attribute>(attributeList);
			}
			else
			{
				// TODO: Merge positional attributes and key/values
				for (int index = 0; index < attributeList.Count; index++)
				{
					var attribute = attributeList[index];
					Add(attribute);
				}
			}

			return this;
		}

		public void Add(Title title)
		{
			if (title != null)
			{
				Title = title;
			}
		}

		public void Add(Anchor item)
		{
			if (item != null)
			{
				Anchor = item;
			}
		}

		public void Add(Attribute item)
		{
			if (item == null)
			{
				return;
			}
			if (_attributes == null)
			{
				_attributes = new List<Attribute> { item };
			}
			else
			{
				_attributes.Add(item);
			}
		}

		public void Add(IEnumerable<Attribute> items)
		{
			if (items == null)
			{
				return;
			}
			if (_attributes == null)
			{
				_attributes = new List<Attribute>(items);
			}
			else
			{
				_attributes.AddRange(items);
			}
		}

		public void Clear() => _attributes?.Clear();

		public bool Contains(Attribute item) => _attributes != null && _attributes.Contains(item);

		public void CopyTo(Attribute[] array, int arrayIndex) => _attributes?.CopyTo(array, arrayIndex);

		public IEnumerator<Attribute> GetEnumerator() => _attributes?.GetEnumerator() ?? Enumerable.Empty<Attribute>().GetEnumerator();

		public int IndexOf(Attribute item) => _attributes?.IndexOf(item) ?? -1;

		public void Insert(int index, Attribute item)
		{
			if (_attributes == null)
			{
				_attributes = new List<Attribute>();
			}

			_attributes.Insert(index, item);
		}

		public bool Remove(Attribute item) => _attributes != null && _attributes.Remove(item);

		public void RemoveAt(int index) => _attributes?.RemoveAt(index);

		protected bool Equals(AttributeList other)
		{
			return AttributesEqual(_attributes, other._attributes) &&
			       Equals(Anchor, other.Anchor) &&
			       Equals(Title, other.Title);
		}

		private static bool AttributesEqual(List<Attribute> attributes, List<Attribute> other)
		{
			if (attributes == null && other == null)
			{
				return true;
			}
			if (attributes == null || other == null)
			{
				return false;
			}

			return attributes.Count == other.Count &&
			       attributes.SequenceEqual(other);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}