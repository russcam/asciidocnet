namespace AsciiDocNet
{
    // TODO: Id must be a valid value. Check in Id setter
    /// <summary>
    /// Used to specify hypertext link targets
    /// </summary>
    public class Anchor
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Anchor"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Anchor(string id) : this(id, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Anchor"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="xRefLabel">The x reference label.</param>
        public Anchor(string id, string xRefLabel)
		{
			Id = id;
			XRefLabel = xRefLabel;
		}

        /// <summary>
        /// A unique string that conforms to the output markup’s anchor syntax
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Text to be displayed by captionless xref macros that refer to this anchor.
        /// This is only really useful when generating DocBook output.
        /// </summary>
        /// <value>
        /// The x reference label.
        /// </value>
        public string XRefLabel { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Anchor left, Anchor right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Anchor left, Anchor right) => !Equals(left, right);

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.VisitAnchor(this);
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

			return obj.GetType() == this.GetType() && Equals((Anchor)obj);
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
				return ((Id?.GetHashCode() ?? 0) * 397) ^ (XRefLabel?.GetHashCode() ?? 0);
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="Anchor" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Anchor other) => 
			string.Equals(Id, other.Id) && string.Equals(XRefLabel, other.XRefLabel);
	}
}