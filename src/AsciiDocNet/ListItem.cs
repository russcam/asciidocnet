namespace AsciiDocNet
{
    /// <summary>
    /// An item in a list.
    /// </summary>
    /// <seealso cref="AsciiDocNet.Container" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public abstract class ListItem : Container, IAttributable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ListItem"/> class.
        /// </summary>
        protected ListItem() : this(1) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItem"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        protected ListItem(int level)
		{
			Level = level;
		}

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public AttributeList Attributes { get; } = new AttributeList();

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public int Level { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ListItem left, ListItem right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ListItem left, ListItem right) => !Equals(left, right);

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

			return obj.GetType() == this.GetType() && Equals((ListItem)obj);
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
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ Level;
				return hashCode;
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="ListItem" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(ListItem other) => 
			base.Equals(other) &&
		    Equals(Attributes, other.Attributes) &&
		    Level == other.Level;
	}
}