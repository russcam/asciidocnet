namespace AsciiDocNet
{
	public class AttributeReference : IInlineElement, IText
	{
		public AttributeReference(string text)
		{
			Text = text;
		}

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}