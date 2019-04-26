using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AsciiDocNet
{
    /// <summary>
    /// An element that contains other elements
    /// </summary>
    public abstract class Container : IList<IElement>
	{
        /// <summary>
        /// The elements within the container
        /// </summary>
        protected List<IElement> Elements;

        /// <summary>
        /// Gets the number of elements contained in the container
        /// </summary>
        public int Count => Elements?.Count ?? 0;

        /// <summary>
        /// Gets a value indicating whether the container is read-only.
        /// </summary>
        bool ICollection<IElement>.IsReadOnly => false;

        /// <summary>
        /// Gets or sets the <see cref="IElement"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="IElement"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Container left, Container right)
		{
			return Equals(left, right);
		}

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Container left, Container right)
		{
			return !Equals(left, right);
		}

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor;

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
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return obj.GetType() == this.GetType() && Equals((Container)obj);
		}

        // TODO: Fix hashcode
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => Elements?.GetHashCode() ?? 0;

        /// <summary>
        /// Adds an element to the container
        /// </summary>
        /// <param name="item">The element to add</param>
        public void Add(IElement item)
		{
			if (Elements == null)
			{
				Elements = new List<IElement>();
			}

			Elements.Add(item);
			item.Parent = this;
		}

        /// <summary>
        /// Removes all elements from the container
        /// </summary>
        public void Clear()
		{
		    if (Elements != null)
		    {
		        foreach (var element in Elements)
		        {
		            element.Parent = null;
		        }

		        Elements.Clear();
		    }
		}

        /// <summary>
        /// Determines whether the container contains a specific element
        /// </summary>
        /// <param name="item">The element to locate</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the element; otherwise, false.
        /// </returns>
        public bool Contains(IElement item) => Elements != null && Elements.Contains(item);

        /// <summary>
        /// Copies the elements of the container to an <see cref="T:System.Array" />, 
        /// starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(IElement[] array, int arrayIndex) => Elements?.CopyTo(array, arrayIndex);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IElement> GetEnumerator() => 
            Elements?.GetEnumerator() ?? Enumerable.Empty<IElement>().GetEnumerator();

        /// <summary>
        /// Determines the index of a specific item in the container.
        /// </summary>
        /// <param name="item">The element to locate in this element.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(IElement item) => Elements?.IndexOf(item) ?? -1;

        /// <summary>
        /// Inserts an element into the container at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert</param>
        public void Insert(int index, IElement item)
		{
			if (Elements == null)
			{
				Elements = new List<IElement>();
			}

			Elements.Insert(index, item);
			item.Parent = this;
		}

        /// <summary>
        /// Removes the first occurrence of a specific element from the container
        /// </summary>
        /// <param name="item">The element to remove</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed; otherwise, false. 
        /// This method also returns false if <paramref name="item" /> is not found.
        /// </returns>
        public bool Remove(IElement item)
		{
			var remove = Elements != null && Elements.Remove(item);

			if (remove)
			{
				item.Parent = null;
			}

			return remove;
		}

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
		{
			if (Elements != null && Elements.Count > index)
			{
				var item = Elements[index];
				item.Parent = null;
				Elements.RemoveAt(index);
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="Container" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Container other)
        {
	        if (Elements == null && other.Elements == null)
		        return true;

	        if (Elements == null && other.Elements != null || Elements != null && other.Elements == null) 
		        return false;

	        return Elements.Count == other.Elements.Count && Elements.SequenceEqual(other.Elements);
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