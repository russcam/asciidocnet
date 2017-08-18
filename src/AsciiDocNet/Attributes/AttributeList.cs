using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// A collection of <see cref="Attribute" />s applied to an <see cref="IElement" />
    /// </summary>
    public class AttributeList : IList<Attribute>
	{
		private List<Attribute> _attributes;
		private bool _isDiscrete;
		private bool _isFloating;

        /// <summary>
        /// Gets a value indicating whether the element to which this instance applies, is discrete.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is discrete; otherwise, <c>false</c>.
        /// </value>
        public bool IsDiscrete
		{
			get { return _isDiscrete; }
			internal set
			{
				_isDiscrete = value;
			    if (value) IsFloating = false;
			}
		}

        /// <summary>
        /// Gets a value indicating whether the element to which this instance applies, is floating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is discrete; otherwise, <c>false</c>.
        /// </value>
		public bool IsFloating
		{
			get { return _isFloating; }
			internal set
			{
				_isFloating = value;
			    if (value) IsDiscrete = false;
			}
		}

        /// <summary>
        /// Gets or sets the anchor.
        /// </summary>
        /// <value>
        /// The anchor.
        /// </value>
        public Anchor Anchor { get; set; }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count => _attributes?.Count ?? 0;

        /// <summary>
        /// Gets a value indicating whether this instance has an anchor.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has anchor; otherwise, <c>false</c>.
        /// </value>
        public bool HasAnchor => Anchor != null;

        /// <summary>
        /// Gets a value indicating whether this instance has a title.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has title; otherwise, <c>false</c>.
        /// </value>
        public bool HasTitle => Title != null;

        /// <summary>
        /// Gets a value indicating whether the attribute collection is read-only.
        /// </summary>
        bool ICollection<Attribute>.IsReadOnly => false;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public Title Title { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Attribute"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Attribute"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Attribute this[int index]
		{
			get { return _attributes?[index]; }
			set
			{
			    if (_attributes == null)
			        _attributes = new List<Attribute>();

			    _attributes[index] = value;
			}
		}

        /// <summary>
        /// Gets or sets the <see cref="Attribute"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="Attribute"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
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
			        _attributes = new List<Attribute>();

			    _attributes.Add(value);
			}
		}

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(AttributeList left, AttributeList right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(AttributeList left, AttributeList right) => !Equals(left, right);

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
		{
		    if (ReferenceEquals(null, obj))
		        return false;
		    if (ReferenceEquals(this, obj))
		        return true;

		    return obj.GetType() == this.GetType() && Equals((AttributeList)obj);
		}

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = _attributes?.GetHashCode() ?? 0;
				hashCode = (hashCode * 397) ^ (Anchor?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ IsDiscrete.GetHashCode();
				hashCode = (hashCode * 397) ^ IsFloating.GetHashCode();
				return hashCode;
			}
		}

        /// <summary>
        /// Adds the attribute list to this instance
        /// </summary>
        /// <param name="attributeList">The attribute list.</param>
        /// <returns></returns>
        public AttributeList Add(AttributeList attributeList)
		{
            if (attributeList == null) return this;

            Add(attributeList.Title);
			Add(attributeList.Anchor);

            if (attributeList.IsDiscrete)
                this.IsDiscrete = true;
            if (attributeList.IsFloating)
                this.IsFloating = true;

            if (_attributes == null)
                _attributes = new List<Attribute>(attributeList);
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

        /// <summary>
        /// Adds the title to this instance
        /// </summary>
        /// <param name="title">The title.</param>
        public void Add(Title title)
		{
            if (title != null)
                Title = title;
		}

        /// <summary>
        /// Adds the anchor to this instance
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        public void Add(Anchor anchor)
		{
            if (anchor != null)
                Anchor = anchor;
		}

        /// <summary>
        /// Adds the attribute to this instance
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        public void Add(Attribute attribute)
		{
		    if (attribute == null) return;
		    if (_attributes == null)
                _attributes = new List<Attribute> {attribute};
		    else
                _attributes.Add(attribute);
		}

        /// <summary>
        /// Adds the attributes to this instance
        /// </summary>
        /// <param name="attributes">The attributes to add.</param>
        public void Add(IEnumerable<Attribute> attributes)
		{
		    if (attributes == null) return;
		    if (_attributes == null)
		        _attributes = new List<Attribute>(attributes);
		    else
		        _attributes.AddRange(attributes);
		}

        /// <summary>
        /// Removes all attributes from the collection.
        /// </summary>
        public void Clear() => _attributes?.Clear();

        /// <summary>
        /// Determines whether the attributes contains a specific value.
        /// </summary>
        /// <param name="item">The attribute to locate.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the 
        /// collection; otherwise, false.
        /// </returns>
        public bool Contains(Attribute item) => _attributes != null && _attributes.Contains(item);

        /// <summary>
        /// Copies the elements of the attributes 
        /// to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination 
        /// of the elements copied from attributes. 
        /// The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(Attribute[] array, int arrayIndex) => _attributes?.CopyTo(array, arrayIndex);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Attribute> GetEnumerator() => _attributes?.GetEnumerator() ?? Enumerable.Empty<Attribute>().GetEnumerator();

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(Attribute item) => _attributes?.IndexOf(item) ?? -1;

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public void Insert(int index, Attribute item)
		{
		    if (_attributes == null)
		        _attributes = new List<Attribute>();

		    _attributes.Insert(index, item);
		}

        /// <summary>
        /// Removes the first occurrence of a specific object from the attributes.
        /// </summary>
        /// <param name="item">The attribute to remove.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the attributes; 
        /// otherwise, false. This method also returns false if 
        /// <paramref name="item" /> is not found in the original attributes.
        /// </returns>
        public bool Remove(Attribute item) => _attributes != null && _attributes.Remove(item);

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index) => _attributes?.RemoveAt(index);

        /// <summary>
        /// Determines whether the specified <see cref="AttributeList" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(AttributeList other) => 
            Equals(Anchor, other.Anchor) &&
            Equals(Title, other.Title) &&
            AttributesEqual(_attributes, other._attributes);

        /// <summary>
        /// Determines if the attribute collections are equal
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        private static bool AttributesEqual(List<Attribute> attributes, List<Attribute> other)
		{
		    if (attributes == null && other == null)
		        return true;
		    if (attributes == null || other == null)
		        return false;

		    return attributes.Count == other.Count &&
			       attributes.SequenceEqual(other);
		}

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}