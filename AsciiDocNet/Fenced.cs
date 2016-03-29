namespace AsciidocNet
{
	public class Fenced : IElement, IAttributable, IText
	{
		public Fenced()
		{
		}

		public Fenced(string text)
		{
			Text = text;
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