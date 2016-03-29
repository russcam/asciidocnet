using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsciidocNet
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
			get { return _attributes?.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }
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

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}