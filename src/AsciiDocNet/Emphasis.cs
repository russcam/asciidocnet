namespace AsciiDocNet
{
    /// <summary>
    /// An emphasis element
    /// </summary>
    /// <seealso cref="AsciiDocNet.IInlineElement" />
    /// <seealso cref="AsciiDocNet.IText" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    /// <example>
    /// A sentence _with_ emphasis
    /// </example>
    /// <example>
    /// Another sentence __with__ emphasis
    /// </example>
    public class Emphasis : IInlineElement, IText, IAttributable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Emphasis"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="doubleDelimited">if set to <c>true</c> [double delimited].</param>
        public Emphasis(string text, bool doubleDelimited = false)
        {
            Text = text;
            DoubleDelimited = doubleDelimited;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emphasis"/> class.
        /// </summary>
        public Emphasis()
        {
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public AttributeList Attributes { get; } = new AttributeList();

        /// <summary>
        /// Gets or sets a value indicating whether [double delimited].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [double delimited]; otherwise, <c>false</c>.
        /// </value>
        public bool DoubleDelimited { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Emphasis left, Emphasis right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Emphasis left, Emphasis right) => !Equals(left, right);

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

            return obj.GetType() == this.GetType() && Equals((Emphasis)obj);
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
                var hashCode = Attributes.GetHashCode();
                hashCode = (hashCode * 397) ^ DoubleDelimited.GetHashCode();
                hashCode = (hashCode * 397) ^ (Text?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
        {
            visitor.VisitEmphasis(this);
            return visitor;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Emphasis" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Emphasis other) => 
            Equals(Attributes, other.Attributes) && 
            DoubleDelimited == other.DoubleDelimited && 
            string.Equals(Text, other.Text);
    }
}