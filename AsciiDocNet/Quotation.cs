using System.Collections.Generic;

namespace AsciidocNet
{
	public class Quotation : IContainerInlineElement, IAttributable
	{
		public Quotation(string text, bool doubleDelimited = false)
		{
			Elements.Add(new TextLiteral(text));
			DoubleDelimited = doubleDelimited;
		}

		public Quotation()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public InlineElementType ContainElementType { get; } =
			InlineElementType.Literal | InlineElementType.AttributeReference | InlineElementType.Emphasis | InlineElementType.EmphasisDouble |
			InlineElementType.ImplicitLink | InlineElementType.InlineAnchor | InlineElementType.InternalAnchor | InlineElementType.Mark |
			InlineElementType.MarkDouble | InlineElementType.Monospace | InlineElementType.MonospaceDouble | InlineElementType.Strong |
			InlineElementType.StrongDouble | InlineElementType.Subscript | InlineElementType.Superscript;

		public bool DoubleDelimited { get; set; }

		public IList<IInlineElement> Elements { get; } = new List<IInlineElement>();

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}