using System;

namespace AsciiDocNet
{
    /// <summary>
    /// A literal element.
    /// <para></para>
    /// Output text is displayed exactly as entered
    /// </summary>
    /// <example>
    /// ....
    /// A literal block
    /// ....
    /// </example>
    /// <example>
    /// [literal]
    /// a literal
    /// </example>
    /// <seealso cref="AsciiDocNet.IElement" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    /// <seealso cref="AsciiDocNet.IText" />
    public class Literal : IElement, IAttributable, IText
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">must be at least one character</exception>
        public Literal(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			if (text.Length == 0)
			{
				throw new ArgumentException("must be at least one character", nameof(text));
			}

			Text = text;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        public Literal()
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
        public static bool operator ==(Literal left, Literal right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Literal left, Literal right) => !Equals(left, right);

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

            return obj.GetType() == this.GetType() && Equals((Literal)obj);
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
				return ((Attributes?.GetHashCode() ?? 0) * 397) ^ (Text?.GetHashCode() ?? 0);
			}
		}

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// The visitor
        /// </returns>
        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.VisitLiteral(this);
			return visitor;
		}

        /// <summary>
        /// Determines whether the specified <see cref="Literal" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Literal other) => 
            Equals(Attributes, other.Attributes) && 
            string.Equals(Text, other.Text);
	}
}