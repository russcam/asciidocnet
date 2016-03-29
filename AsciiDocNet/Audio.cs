namespace AsciidocNet
{
	public class Audio : Media
	{
		public Audio(string path) : base(path)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}