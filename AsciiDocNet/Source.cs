namespace AsciidocNet
{
	public class Source : Listing
	{
		public Source(string text)
		{
			Text = text;
		}

		public Source()
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}