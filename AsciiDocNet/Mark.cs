using System.Collections.Generic;

namespace AsciiDocNet
{
	public class Mark : IContainerInlineElement, IAttributable
	{
		public Mark(string text, bool doubleDelimited = false)
		{
			Elements.Add(new TextLiteral(text));
			DoubleDelimited = doubleDelimited;
		}

		public Mark()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public InlineElementType ContainElementType { get; } =
			InlineElementType.Literal | InlineElementType.AttributeReference | InlineElementType.Emphasis | InlineElementType.EmphasisDouble |
			InlineElementType.ImplicitLink | InlineElementType.InlineAnchor | InlineElementType.InternalAnchor | InlineElementType.Monospace |
			InlineElementType.MonospaceDouble | InlineElementType.Strong |
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