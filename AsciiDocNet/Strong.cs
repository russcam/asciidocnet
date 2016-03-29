using System.Collections.Generic;

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

		public bool DoubleDelimited { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		// TODO: restrict thr inline elements that can be added based on ContainElementType
		public IList<IInlineElement> Elements { get; } = new List<IInlineElement>();

		public InlineElementType ContainElementType { get; } =
			InlineElementType.Literal | InlineElementType.AttributeReference | InlineElementType.Emphasis | InlineElementType.EmphasisDouble |
			InlineElementType.ImplicitLink | InlineElementType.InlineAnchor | InlineElementType.InternalAnchor | InlineElementType.Mark |
			InlineElementType.MarkDouble | InlineElementType.Monospace | InlineElementType.MonospaceDouble | InlineElementType.Quotation |
			InlineElementType.QuotationDouble | InlineElementType.Subscript | InlineElementType.Superscript;
	}
}