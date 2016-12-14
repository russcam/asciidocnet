namespace AsciiDocNet
{
	public class CheckListItem : UnorderedListItem
	{
		public CheckListItem(int level, bool @checked) : base(level)
		{
			Checked = @checked;
		}

		public CheckListItem(int level) : base(level)
		{
		}

		public CheckListItem() : this(1)
		{
		}

		public bool Checked { get; set; }

		public static bool operator ==(CheckListItem left, CheckListItem right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(CheckListItem left, CheckListItem right)
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

			return obj.GetType() == this.GetType() && Equals((CheckListItem)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ Checked.GetHashCode();
			}
		}

		protected bool Equals(CheckListItem other)
		{
			return base.Equals(other) && Checked == other.Checked;
		}
	}
}