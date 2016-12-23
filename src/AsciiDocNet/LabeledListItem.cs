namespace AsciiDocNet
{
    /// <summary>
    /// A labeled list item in a <see cref="LabeledList"/>
    /// </summary>
    /// <seealso cref="AsciiDocNet.ListItem" />
    public class LabeledListItem : ListItem
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledListItem"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="level">The level.</param>
        public LabeledListItem(string label, int level) : base(level)
		{
			Label = label;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledListItem"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public LabeledListItem(string label) : this(label, 1)
		{
		}

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(LabeledListItem left, LabeledListItem right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(LabeledListItem left, LabeledListItem right) => !Equals(left, right);

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitLabeledListItem(this);
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

            return obj.GetType() == this.GetType() && Equals((LabeledListItem)obj);
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
				return (base.GetHashCode() * 397) ^ (Label?.GetHashCode() ?? 0);
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="LabeledListItem" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(LabeledListItem other) => 
            base.Equals(other) && 
            string.Equals(Label, other.Label);
	}
}