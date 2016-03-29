namespace AsciiDocNet
{
	public class Title
	{
		public string Text { get; set; }

		public Title(string text)
		{
			Text = text;
		}

		public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}