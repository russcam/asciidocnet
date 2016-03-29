using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciidocNet
{
	public class Paragraph : IElement, IAttributable, IContainerInlineElement
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

			Elements = elements;
		}

		public Paragraph(IInlineElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			Elements.Add(element);
		}

		public Paragraph(string text) : this(new TextLiteral(text))
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public IList<IInlineElement> Elements { get; } = new List<IInlineElement>();

		public InlineElementType ContainElementType { get; } = InlineElementType.All;

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public Container Parent { get; set; }
	}
}