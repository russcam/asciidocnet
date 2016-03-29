using System.Collections.Generic;

namespace AsciidocNet
{
	public class Monospace : IContainerInlineElement, IAttributable
	{
		public Monospace(string text)
		{
			Elements.Add(new TextLiteral(text));
		}

		public Monospace()
		{
		}

		public bool DoubleDelimited { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public IList<IInlineElement> Elements { get; } = new List<IInlineElement>();

		public InlineElementType ContainElementType { get; } =
			InlineElementType.Literal | InlineElementType.AttributeReference | InlineElementType.Emphasis | InlineElementType.EmphasisDouble |
			InlineElementType.InlineAnchor | InlineElementType.InternalAnchor | InlineElementType.Mark |
			InlineElementType.MarkDouble | InlineElementType.Strong |
			InlineElementType.StrongDouble | InlineElementType.Subscript | InlineElementType.Superscript;
	}
}