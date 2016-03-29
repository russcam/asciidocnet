namespace AsciiDocNet
{
	public class Image : Media
	{
		public Image(string path) : base(path)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}