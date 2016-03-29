namespace AsciidocNet
{
	public class CheckListItem : ListItem
	{
		public CheckListItem(int level) : base(level)
		{
		}

		public CheckListItem()
		{
		}

		public bool Checked { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}