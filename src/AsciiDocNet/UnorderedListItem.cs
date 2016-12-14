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

		public static bool operator ==(UnorderedListItem left, UnorderedListItem right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(UnorderedListItem left, UnorderedListItem right)
		{
			return !Equals(left, right);
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((UnorderedListItem)obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		protected bool Equals(UnorderedListItem other)
		{
			return base.Equals(other);
		}
	}
}