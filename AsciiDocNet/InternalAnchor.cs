namespace AsciidocNet
{
	public class InternalAnchor : InlineAnchor
	{
		public InternalAnchor(string id, string xRefLabel) : base(id, xRefLabel)
		{
		}

		public InternalAnchor(string id) : base(id)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}