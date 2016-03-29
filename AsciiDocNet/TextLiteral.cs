namespace AsciiDocNet
{
	public class TextLiteral : IInlineElement, IAttributable, IText
	{
		public TextLiteral(string text)
		{
			Text = text;
		}

		public TextLiteral()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}