using System.Linq;

namespace AsciiDocNet
{
	public class Strong : InlineContainer, IInlineElement, IAttributable
	{
		public Strong(string text, bool doubleDelimited = false)
		{
			Add(new TextLiteral(text));
			DoubleDelimited = doubleDelimited;
		}

		public Strong()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public override InlineElementType ContainElementType { get; } =
			InlineElementType.Literal | InlineElementType.AttributeReference | InlineElementType.Emphasis | InlineElementType.EmphasisDouble |
			InlineElementType.ImplicitLink | InlineElementType.InlineAnchor | InlineElementType.InternalAnchor | InlineElementType.Mark |
			InlineElementType.MarkDouble | InlineElementType.Monospace | InlineElementType.MonospaceDouble | InlineElementType.Quotation |
			InlineElementType.QuotationDouble | InlineElementType.Subscript | InlineElementType.Superscript;

		public bool DoubleDelimited { get; set; }

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
			return Equals((Strong)obj);
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

		public static bool operator ==(Strong left, Strong right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Strong left, Strong right)
		{
			return !Equals(left, right);
		}

		protected bool Equals(Strong other)
		{
			return DoubleDelimited == other.DoubleDelimited &&
			       Attributes.Equals(other.Attributes) &&
			       Elements.SequenceEqual(other.Elements);
		}
	}
}