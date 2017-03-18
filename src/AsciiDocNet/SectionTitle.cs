using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// A Section title
    /// </summary>
    /// <seealso cref="AsciiDocNet.InlineContainer" />
    /// <seealso cref="AsciiDocNet.IElement" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public class SectionTitle : InlineContainer, IElement, IAttributable
	{
		private int _level;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionTitle"/> class.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="level">The level.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">must have at least one element</exception>
        public SectionTitle(IList<IInlineElement> elements, int level)
		{
			if (elements == null)
			{
				throw new ArgumentNullException(nameof(elements));
			}
			if (!elements.Any())
			{
				throw new ArgumentException("must have at least one element", nameof(elements));
			}

			ValidateLevel(level);
			Elements = elements.ToList();
			_level = level;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionTitle"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="level">The level.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public SectionTitle(IInlineElement element, int level)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			ValidateLevel(level);
			Add(element);
			_level = level;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionTitle"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="level">The level.</param>
        public SectionTitle(string text, int level) : this(new TextLiteral(text), level)
		{
		}

        /// <summary>
        /// Gets or sets a value indicating whether this section title is discrete.
        /// A discrete title is styled like a section Title but is not part of the content hierarchy 
        /// (i.e. it ignores section nesting rules). It will also not be included in the Table of Contents
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is discrete; otherwise, <c>false</c>.
        /// </value>
        public bool IsDiscrete
		{
			get { return Attributes.IsDiscrete; }
			set { Attributes.IsDiscrete = value; }
		}

        /// <summary>
        /// Gets or sets a value indicating whether this section title is floating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is floating; otherwise, <c>false</c>.
        /// </value>
        public bool IsFloating
		{
			get { return Attributes.IsFloating; }
			set { Attributes.IsFloating = value; }
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
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public int Level
		{
			get { return _level; }
			set
			{
				ValidateLevel(value);
				_level = value;
			}
		}

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
			visitor.VisitSectionTitle(this);
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

			return obj.GetType() == this.GetType() && Equals((SectionTitle)obj);
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
				var hashCode = _level;
				hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ Elements.GetHashCode();
				return hashCode;
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
        public static bool operator ==(SectionTitle left, SectionTitle right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(SectionTitle left, SectionTitle right) => !Equals(left, right);

        /// <summary>
        /// Determines whether the specified <see cref="SectionTitle" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(SectionTitle other) => 
            _level == other._level &&
		    Attributes.Equals(other.Attributes) &&
		    Elements.SequenceEqual(other.Elements);

        /// <summary>
        /// Validates the level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">must be between 1 and 6</exception>
        private static void ValidateLevel(int level)
		{
			if (level < 1 || level > 6)
			{
				throw new ArgumentOutOfRangeException(nameof(level), "must be between 1 and 6");
			}
		}
	}
}