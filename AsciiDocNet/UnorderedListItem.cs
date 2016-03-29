namespace AsciiDocNet
{
	public class UnorderedListItem : ListItem
	{
		public UnorderedListItem() : this(1)
		{
		}

		public UnorderedListItem(int level) : base(level)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}