namespace AsciiDocNet
{
	public abstract class ListItem : Container, IAttributable
	{
		protected ListItem() : this(1)
		{
		}

		protected ListItem(int level)
		{
			Level = level;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public int Level { get; set; }
	}
}