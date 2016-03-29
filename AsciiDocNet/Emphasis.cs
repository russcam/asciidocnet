namespace AsciiDocNet
{
	public class Emphasis : IInlineElement, IText, IAttributable
	{
		public Emphasis(string text, bool doubleDelimited = false)
		{
			Text = text;
			DoubleDelimited = doubleDelimited;
		}

		public Emphasis()
		{
		}

		public bool DoubleDelimited { get; set; }

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public AttributeList Attributes { get; } = new AttributeList();
	}
}