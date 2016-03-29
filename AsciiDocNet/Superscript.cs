namespace AsciiDocNet
{
	public class Superscript : IInlineElement, IText, IAttributable
	{
		public Superscript(string text)
		{
			Text = text;
		}

		public Superscript()
		{
		}

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public AttributeList Attributes { get; } = new AttributeList();
	}
}