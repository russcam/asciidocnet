namespace AsciiDocNet
{
	public class InlineAnchor : Anchor, IInlineElement
	{
		public InlineAnchor(string id, string xRefLabel) : base(id, xRefLabel)
		{
		}

		public InlineAnchor(string id) : base(id)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}