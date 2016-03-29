namespace AsciiDocNet
{
	public class Anchor
	{
		public string Id { get; set; }

		public Anchor(string id) : this(id, null)
		{
		}

		public Anchor(string id, string xRefLabel)
		{
			Id = id;
			XRefLabel = xRefLabel;
		}

		public string XRefLabel { get; set; }

		public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor :IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}