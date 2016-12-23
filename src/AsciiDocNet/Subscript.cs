namespace AsciiDocNet
{
    /// <summary>
    /// A subscript element.
    /// </summary>
    /// <example>
    /// ~word~
    /// </example>
    /// <seealso cref="AsciiDocNet.IInlineElement" />
    /// <seealso cref="AsciiDocNet.IText" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public class Subscript : IInlineElement, IText, IAttributable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subscript"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public Subscript(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscript"/> class.
        /// </summary>
        public Subscript()
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
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// the visitor
        /// </returns>
        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
        {
            visitor.VisitSubscript(this);
            return visitor;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Subscript" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Subscript other) => string.Equals(Text, other.Text) && Equals(Attributes, other.Attributes);

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

            return obj.GetType() == this.GetType() && Equals((Subscript)obj);
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
                return ((Text?.GetHashCode() ?? 0) * 397) ^ Attributes.GetHashCode();
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
        public static bool operator ==(Subscript left, Subscript right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Subscript left, Subscript right) => !Equals(left, right);
    }
}