namespace AsciiDocNet
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

		public static bool operator ==(LabeledListItem left, LabeledListItem right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(LabeledListItem left, LabeledListItem right)
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
			return Equals((LabeledListItem)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ (Label?.GetHashCode() ?? 0);
			}
		}

		protected bool Equals(LabeledListItem other)
		{
			return base.Equals(other) && string.Equals(Label, other.Label);
		}
	}
}