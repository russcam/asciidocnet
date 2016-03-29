namespace AsciiDocNet
{
	public class Pass : IElement, IText, IAttributable
	{
		public Pass(string text)
		{
			Text = text;
		}

		public Pass()
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