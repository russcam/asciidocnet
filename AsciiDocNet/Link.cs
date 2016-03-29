namespace AsciidocNet
{
	public class Link : IInlineElement, IAttributable
	{
		public Link(string href, string text)
		{
			Href = href;
			Text = text;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Href { get; set; }

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}