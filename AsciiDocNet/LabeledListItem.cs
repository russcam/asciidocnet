namespace AsciidocNet
{
	public class LabeledListItem : ListItem
	{
		public LabeledListItem(string label, int level) : base(level)
		{
			Label = label;
		}

		public LabeledListItem(string label) : this(label, 1)
		{
		}

		public string Label { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}