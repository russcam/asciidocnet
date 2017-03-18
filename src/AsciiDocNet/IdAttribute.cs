namespace AsciiDocNet
{
    /// <summary>
    /// Specifies a unique name for an element. That name can only be used once in the document.
    /// </summary>
    /// <example>
    /// [#goals]
    /// * Goal 1
    /// * Goal 2
    /// </example>
    /// <example>
    /// [#free_the_world]*free the world*
    /// </example>
    /// <seealso cref="AsciiDocNet.NamedAttribute" />
    public class IdAttribute : NamedAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="singleQuoted">if set to <c>true</c> [single quoted].</param>
        public IdAttribute(string value, bool singleQuoted) : base("id", value, singleQuoted)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="singleQuoted">if set to <c>true</c> [single quoted].</param>
        public IdAttribute(string value, string reference, bool singleQuoted) : base("id", value, singleQuoted)
		{
			Reference = reference;
		}

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public string Reference { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(IdAttribute left, IdAttribute right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(IdAttribute left, IdAttribute right) => !Equals(left, right);

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitNamedAttribute(this);
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

			return obj.GetType() == this.GetType() && Equals((IdAttribute)obj);
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
				return (base.GetHashCode() * 397) ^ (Reference?.GetHashCode() ?? 0);
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="IdAttribute" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(IdAttribute other) => base.Equals(other) && string.Equals(Reference, other.Reference);
	}
}