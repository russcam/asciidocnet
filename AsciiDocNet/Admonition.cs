namespace AsciidocNet
{
	public class Admonition : Container, IElement, IAttributable
	{
		public Admonition(AdmonitionStyle style)
		{
			Style = style;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public AdmonitionStyle Style { get; set; }

		public Container Parent { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}