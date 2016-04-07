namespace AsciiDocNet
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

		public static bool operator ==(OrderedListItem left, OrderedListItem right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(OrderedListItem left, OrderedListItem right)
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
			return Equals((OrderedListItem)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ Number.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)Numbering;
				return hashCode;
			}
		}

		protected bool Equals(OrderedListItem other)
		{
			return base.Equals(other) && Number == other.Number && Numbering == other.Numbering;
		}
	}
}