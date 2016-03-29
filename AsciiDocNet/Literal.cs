using System;

namespace AsciidocNet
{
	public class Literal : IElement, IAttributable, IText
	{
		public Literal(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			if (text.Length == 0)
			{
				throw new ArgumentException("must be at least one character", nameof(text));
			}

			Text = text;
		}

		public Literal()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public Container Parent { get; set; }
	}
}