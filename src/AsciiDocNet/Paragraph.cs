using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// A paragraph element.
    /// </summary>
    /// <example>
    /// This is a paragraph.
    /// 
    /// And this is another paragraph
    /// which continues on two lines.
    /// </example>
    /// <seealso cref="AsciiDocNet.InlineContainer" />
    /// <seealso cref="AsciiDocNet.IElement" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public class Paragraph : InlineContainer, IElement, IAttributable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Paragraph"/> class.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">must have at least one element</exception>
        public Paragraph(IList<IInlineElement> elements)
		{
			if (elements == null)
			{
				throw new ArgumentNullException(nameof(elements));
			}
			if (!elements.Any())
			{
				throw new ArgumentException("must have at least one element", nameof(elements));
			}

			Elements = elements.ToList();
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Paragraph"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Paragraph(IInlineElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			Add(element);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Paragraph"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public Paragraph(string text) : this(new TextLiteral(text))
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
        /// Gets the types of elements that this inline container can contain
        /// </summary>
        /// <value>
        /// The type of elements
        /// </value>
        public override InlineElementType ContainElementType { get; } = InlineElementType.All;

        /// <summary>
        /// Gets or sets the parent element
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Container Parent { get; set; }

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// The visitor
        /// </returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitParagraph(this);
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
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == this.GetType() && Equals((Paragraph)obj);
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
				return (Attributes.GetHashCode() * 397) ^ Elements.GetHashCode();
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
        public static bool operator ==(Paragraph left, Paragraph right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Paragraph left, Paragraph right) => !Equals(left, right);

        /// <summary>
        /// Determines whether the specified <see cref="Paragraph" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Paragraph other) => 
            Equals(Attributes, other.Attributes) && 
            Elements.SequenceEqual(other.Elements);
	}
}