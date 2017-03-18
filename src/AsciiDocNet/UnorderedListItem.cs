namespace AsciiDocNet
{
    /// <summary>
    /// An item in an <see cref="UnorderedList"/>
    /// </summary>
    /// <example>
    /// * This is an unordered list item
    /// </example>
    /// <seealso cref="AsciiDocNet.ListItem" />
    public class UnorderedListItem : ListItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnorderedListItem"/> class.
        /// </summary>
        public UnorderedListItem() : this(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnorderedListItem"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public UnorderedListItem(int level) : base(level)
        {
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(UnorderedListItem left, UnorderedListItem right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(UnorderedListItem left, UnorderedListItem right) => !Equals(left, right);

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitUnorderedListItem(this);
            return visitor;
        }

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
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && Equals((UnorderedListItem)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Determines whether the specified <see cref="UnorderedListItem" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(UnorderedListItem other) => base.Equals(other);
    }
}