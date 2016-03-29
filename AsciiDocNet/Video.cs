namespace AsciidocNet
{
	public class Video : Media
	{
		public Video(string path) : base(path)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}