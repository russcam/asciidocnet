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
        /// <summary>
        /// The elements
        /// </summary>
        protected List<IInlineElement> Elements;

        /// <summary>
        /// Gets the types of elements that this inline container can contain
        /// </summary>
        /// <value>
        /// The type of elements
        /// </value>
        public abstract InlineElementType ContainElementType { get; }

        /// <summary>
        /// Gets the number of elements contained in the inline container.
        /// </summary>
        public int Count => Elements?.Count ?? 0;

        /// <summary>
        /// Gets a value indicating whether the inline container is read-only.
        /// </summary>
        bool ICollection<IInlineElement>.IsReadOnly => false;

        /// <summary>
        /// Gets or sets the <see cref="IInlineElement"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="IInlineElement"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public IInlineElement this[int index]
		{
			get { return Elements?[index]; }
			set
			{
			    if (Elements == null)
			        Elements = new List<IInlineElement>();

			    Elements[index] = value;
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
        public static bool operator ==(InlineContainer left, InlineContainer right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(InlineContainer left, InlineContainer right) => !Equals(left, right);

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The visitor</returns>
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

			return obj.GetType() == this.GetType() && Equals((InlineContainer)obj);
		}

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => Elements.GetHashCode();

        /// <summary>
        /// Adds an element to the inline container.
        /// </summary>
        /// <param name="item">The element to add.</param>
        public void Add(IInlineElement item)
		{
		    if (Elements == null) Elements = new List<IInlineElement>();
		    Elements.Add(item);
		}

        /// <summary>
        /// Removes all elements from the inline container.
        /// </summary>
        public void Clear() => Elements?.Clear();

        /// <summary>
        /// Determines whether the inline container contains a specific value.
        /// </summary>
        /// <param name="item">The element to locate.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found; otherwise, false.
        /// </returns>
        public bool Contains(IInlineElement item) => Elements != null && Elements.Contains(item);

        /// <summary>
        /// Copies the elements of the inline container to an <see cref="T:System.Array" />, 
        /// starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> 
        /// that is the destination of the elements copied from inline container. 
        /// The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(IInlineElement[] array, int arrayIndex) => Elements?.CopyTo(array, arrayIndex);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IInlineElement> GetEnumerator() => 
			Elements?.GetEnumerator() ?? Enumerable.Empty<IInlineElement>().GetEnumerator();

        /// <summary>
        /// Determines the index of a specific element in the inline container.
        /// </summary>
        /// <param name="item">The element to locate.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found; otherwise, -1.
        /// </returns>
        public int IndexOf(IInlineElement item) => Elements?.IndexOf(item) ?? -1;

        /// <summary>
        /// Inserts an element to the inline container at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The element to insert.</param>
        public void Insert(int index, IInlineElement item)
		{
			if (Elements == null)
			{
				Elements = new List<IInlineElement>();
			}

			Elements.Insert(index, item);
		}

        /// <summary>
        /// Removes the first occurrence of a specific element from the inline container.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed; otherwise, false.
        /// This method also returns false if <paramref name="item" /> is not found in the inline container.
        /// </returns>
        public bool Remove(IInlineElement item) => Elements != null && Elements.Remove(item);

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index) => Elements?.RemoveAt(index);

        /// <summary>
        /// Determines whether the specified <see cref="InlineContainer" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(InlineContainer other) => 
			Elements.Count == other.Elements.Count && Elements.SequenceEqual(other.Elements);

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}