namespace AsciiDocNet
{
	public class Subscript : IInlineElement, IText, IAttributable
	{
		public Subscript(string text)
		{
			Text = text;
		}

		public Subscript()
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