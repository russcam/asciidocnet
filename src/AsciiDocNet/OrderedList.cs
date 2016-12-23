using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// An ordered list.
    /// </summary>
    /// <example>
    /// . List item 1
    /// . List item 2
    /// . List item 3
    /// </example>
    /// <example>
    /// 1. List item 1
    /// 2. List item 2
    /// 3. List item 3
    /// </example>
    /// <seealso cref="AsciiDocNet.IElement" />
    public class OrderedList : IElement
	{
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IList<OrderedListItem> Items { get; } = new List<OrderedListItem>();

        /// <summary>
        /// Gets or sets the parent element
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Container Parent { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(OrderedList left, OrderedList right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(OrderedList left, OrderedList right) => !Equals(left, right);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
		{
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && Equals((OrderedList)obj);
		}

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => Items.GetHashCode();

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// The visitor
        /// </returns>
        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.VisitOrderedList(this);
			return visitor;
		}

        /// <summary>
        /// Determines whether the specified <see cref="OrderedList" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(OrderedList other) => Items.SequenceEqual(other.Items);
	}
}