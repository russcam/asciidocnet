using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class Paragraph : InlineContainer, IElement, IAttributable
	{
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

		public Paragraph(IInlineElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			Add(element);
		}

		public Paragraph(string text) : this(new TextLiteral(text))
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public override InlineElementType ContainElementType { get; } = InlineElementType.All;

		public Container Parent { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}

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
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((Paragraph)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Attributes.GetHashCode() * 397) ^ Elements.GetHashCode();
			}
		}

		public static bool operator ==(Paragraph left, Paragraph right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Paragraph left, Paragraph right)
		{
			return !Equals(left, right);
		}

		protected bool Equals(Paragraph other)
		{
			return Equals(Attributes, other.Attributes) && Elements.SequenceEqual(other.Elements);
		}
	}
}