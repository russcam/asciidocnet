namespace AsciidocNet
{
	public class OrderedListItem : ListItem
	{
		public OrderedListItem(int level) : base(level)
		{
		}

		public OrderedListItem() : this(1)
		{
		}

		public int? Number { get; set; }

		public NumberStyle Numbering { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}