namespace AsciiDocNet
{
    /// <summary>
    /// A link element.
    /// </summary>
    /// <example>
    /// link:url[optional link text, optional target attribute, optional role attribute]
    /// </example>
    /// <example>
    /// 
    /// </example>
    /// <seealso cref="AsciiDocNet.IInlineElement" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public class Link : IInlineElement, IAttributable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="text">The text.</param>
        public Link(string href, string text)
		{
			Href = href;
			Text = text;
		}

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public AttributeList Attributes { get; } = new AttributeList();

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>
        /// The href.
        /// </value>
        public string Href { get; set; }

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
        public static bool operator ==(Link left, Link right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Link left, Link right) => !Equals(left, right);

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

            return obj.GetType() == this.GetType() && Equals((Link)obj);
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
				hashCode = (hashCode * 397) ^ (Href?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Text?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

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
			visitor.VisitLink(this);
			return visitor;
		}

        /// <summary>
        /// Determines whether the specified <see cref="Link" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Link other) => 
            Equals(Attributes, other.Attributes) && 
            string.Equals(Href, other.Href) && 
            string.Equals(Text, other.Text);
	}
}