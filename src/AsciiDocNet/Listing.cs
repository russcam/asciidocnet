using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// A listing element. Used for source code or keyboard input that should be displayed as entered
    /// </summary>
    /// <example>
    /// ----
    /// A listing
    /// ----
    /// </example>
    /// <example>
    /// [listing]
    /// A listing
    /// </example>
    /// <seealso cref="AsciiDocNet.IElement" />
    /// <seealso cref="AsciiDocNet.IText" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public class Listing : IElement, IText, IAttributable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Listing"/> class.
        /// </summary>
        public Listing()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Listing"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public Listing(string text)
		{
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
        /// Gets the callouts.
        /// </summary>
        /// <value>
        /// The callouts.
        /// </value>
        public IList<Callout> Callouts { get; } = new List<Callout>();

        /// <summary>
        /// Gets or sets the parent element
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Container Parent { get; set; }

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
        public static bool operator ==(Listing left, Listing right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Listing left, Listing right) => !Equals(left, right);

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// The visitor
        /// </returns>
        public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.VisitListing(this);
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

			return obj.GetType() == this.GetType() && Equals((Listing)obj);
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
				var hashCode = Attributes?.GetHashCode() ?? 0;
				hashCode = (hashCode * 397) ^ (Callouts?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Text?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(Listing other) =>
            string.Equals(Text, other.Text) &&
            Equals(Attributes, other.Attributes) &&
            Callouts.SequenceEqual(other.Callouts);

	}
}