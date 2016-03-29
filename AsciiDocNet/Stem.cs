namespace AsciiDocNet
{
	public class Stem : IElement, IText, IAttributable
	{
		public Stem(string text)
		{
			Text = text;
		}

		public Stem()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}