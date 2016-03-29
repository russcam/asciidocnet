namespace AsciiDocNet
{
	public class Example : Container, IElement, IAttributable
	{
		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}