using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class Strong : IContainerInlineElement, IAttributable
	{
		public Strong(string text, bool doubleDelimited = false)
		{
			Elements.Add(new TextLiteral(text));
			DoubleDelimited = doubleDelimited;
		}

		public Strong()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public InlineElementType ContainElementType { get; } =
			InlineElementType.Literal | InlineElementType.AttributeReference | InlineElementType.Emphasis | InlineElementType.EmphasisDouble |
			InlineElementType.ImplicitLink | InlineElementType.InlineAnchor | InlineElementType.InternalAnchor | InlineElementType.Mark |
			InlineElementType.MarkDouble | InlineElementType.Monospace | InlineElementType.MonospaceDouble | InlineElementType.Quotation |
			InlineElementType.QuotationDouble | InlineElementType.Subscript | InlineElementType.Superscript;

		public bool DoubleDelimited { get; set; }

		// TODO: restrict the inline elements that can be added based on ContainElementType
		public IList<IInlineElement> Elements { get; } = new List<IInlineElement>();

		public static bool operator ==(Strong left, Strong right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Strong left, Strong right)
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

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Strong other)
		{
			return DoubleDelimited == other.DoubleDelimited &&
			       Attributes.Equals(other.Attributes) &&
			       Elements.SequenceEqual(other.Elements);
		}
	}
}