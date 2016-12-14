using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class Monospace : InlineContainer, IInlineElement, IAttributable
	{
		public Monospace(string text)
		{
			Add(new TextLiteral(text));
		}

		public Monospace()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public override InlineElementType ContainElementType { get; } =
			InlineElementType.Literal | InlineElementType.AttributeReference | InlineElementType.Emphasis | InlineElementType.EmphasisDouble |
			InlineElementType.InlineAnchor | InlineElementType.InternalAnchor | InlineElementType.Mark |
			InlineElementType.MarkDouble | InlineElementType.Strong |
			InlineElementType.StrongDouble | InlineElementType.Subscript | InlineElementType.Superscript;

		public bool DoubleDelimited { get; set; }

		public static bool operator ==(Monospace left, Monospace right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Monospace left, Monospace right)
		{
			return !Equals(left, right);
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
			return Equals((Monospace)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = DoubleDelimited.GetHashCode();
				hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ Elements.GetHashCode();
				return hashCode;
			}
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Monospace other)
		{
			return DoubleDelimited == other.DoubleDelimited &&
			       Attributes.Equals(other.Attributes) &&
			       Elements.SequenceEqual(other.Elements);
		}
	}
}