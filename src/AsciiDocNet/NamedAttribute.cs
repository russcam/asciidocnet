namespace AsciiDocNet
{
    /// <summary>
    /// A named attribute within an <see cref="AttributeList"/>
    /// </summary>
    /// <example>
    /// [named attribute="value"]
    /// </example>    
    /// <example>
    /// [named attribute='value']
    /// </example>
    /// <seealso cref="AsciiDocNet.Attribute" />
    public class NamedAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="singleQuoted">if set to <c>true</c> [single quoted].</param>
        public NamedAttribute(string name, string value, bool singleQuoted) : base(name)
        {
            Value = value;
            SingleQuoted = singleQuoted;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [single quoted].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [single quoted]; otherwise, <c>false</c>.
        /// </value>
        public bool SingleQuoted { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(NamedAttribute left, NamedAttribute right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(NamedAttribute left, NamedAttribute right) => !Equals(left, right);

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

            return obj.GetType() == this.GetType() && Equals((NamedAttribute)obj);
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
                return (base.GetHashCode() * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="NamedAttribute" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(NamedAttribute other) => base.Equals(other) && string.Equals(Value, other.Value);
    }
}