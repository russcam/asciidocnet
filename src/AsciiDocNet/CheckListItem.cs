namespace AsciiDocNet
{
    /// <summary>
    /// A check list item
    /// </summary>
    /// <seealso cref="AsciiDocNet.UnorderedListItem" />
    public class CheckListItem : UnorderedListItem
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListItem"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="checked">if set to <c>true</c> [checked].</param>
        public CheckListItem(int level, bool @checked) : base(level)
		{
			Checked = @checked;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListItem"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public CheckListItem(int level) : base(level)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListItem"/> class.
        /// </summary>
        public CheckListItem() : this(1)
		{
		}

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckListItem"/> is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if checked; otherwise, <c>false</c>.
        /// </value>
        public bool Checked { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(CheckListItem left, CheckListItem right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(CheckListItem left, CheckListItem right) => !Equals(left, right);

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The visitor</returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitCheckListItem(this);
			return visitor;
		}

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

		    return obj.GetType() == this.GetType() && Equals((CheckListItem)obj);
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
				return (base.GetHashCode() * 397) ^ Checked.GetHashCode();
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="CheckListItem" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(CheckListItem other) => base.Equals(other) && Checked == other.Checked;
	}
}