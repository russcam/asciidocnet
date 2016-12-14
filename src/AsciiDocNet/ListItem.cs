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

		public static bool operator ==(ListItem left, ListItem right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ListItem left, ListItem right)
		{
			return !Equals(left, right);
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
			return Equals((ListItem)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ Level;
				return hashCode;
			}
		}

		protected bool Equals(ListItem other)
		{
			return base.Equals(other) &&
			       Equals(Attributes, other.Attributes) &&
			       Level == other.Level;
		}
	}
}