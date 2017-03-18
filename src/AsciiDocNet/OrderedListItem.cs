namespace AsciiDocNet
{
    /// <summary>
    /// An order list item within an <see cref="OrderedList"/>.
    /// </summary>
    /// <example>
    /// . Ordered list item
    /// </example>
    /// <example>
    /// 1. Ordered list item
    /// </example>
    /// <seealso cref="AsciiDocNet.ListItem" />
    public class OrderedListItem : ListItem
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedListItem"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public OrderedListItem(int level) : base(level)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedListItem"/> class.
        /// </summary>
        public OrderedListItem() : this(1)
		{
		}

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public int? Number { get; set; }

        /// <summary>
        /// Gets or sets the numbering.
        /// </summary>
        /// <value>
        /// The numbering.
        /// </value>
        public NumberStyle Numbering { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(OrderedListItem left, OrderedListItem right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(OrderedListItem left, OrderedListItem right) => !Equals(left, right);

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitOrderedListItem(this);
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
		        return false;
		    if (ReferenceEquals(this, obj))
		        return true;

		    return obj.GetType() == this.GetType() && Equals((OrderedListItem)obj);
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
				var hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ Number.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)Numbering;
				return hashCode;
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="OrderedListItem" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(OrderedListItem other) => 
            base.Equals(other) && 
            Number == other.Number && 
            Numbering == other.Numbering;
	}
}