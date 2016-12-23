namespace AsciiDocNet
{
    // TODO: Should derive from AsciiSectionTitle and take a collection of inline elements for title
    /// <summary>
    /// A document title
    /// </summary>
    /// <seealso cref="AsciiDocNet.IAttributable" />
    /// <example>
    /// = This is a title
    /// </example>
    public class DocumentTitle : IAttributable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTitle"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public DocumentTitle(string title) : this(title, null)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTitle"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="subtitle">The subtitle.</param>
        public DocumentTitle(string title, string subtitle)
		{
			Title = title;
			Subtitle = subtitle;
		}

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public AttributeList Attributes { get; } = new AttributeList();

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>
        /// The subtitle.
        /// </value>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(DocumentTitle left, DocumentTitle right)
		{
			return Equals(left, right);
		}

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(DocumentTitle left, DocumentTitle right)
		{
			return !Equals(left, right);
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
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			return obj.GetType() == this.GetType() && Equals((DocumentTitle)obj);
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
				hashCode = (hashCode * 397) ^ (Subtitle?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
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
			visitor.VisitDocumentTitle(this);
			return visitor;
		}

        /// <summary>
        /// Determines whether the specified <see cref="DocumentTitle" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(DocumentTitle other)
		{
			return Equals(Attributes, other.Attributes) &&
			       string.Equals(Subtitle, other.Subtitle) &&
			       string.Equals(Title, other.Title);
		}
	}
}