namespace AsciiDocNet
{
	// TODO: Callout can have other elements
	public class Callout
	{
		public Callout(int number, string text)
		{
			Number = number;
			Text = text;
		}

		public int Number { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Callout left, Callout right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Callout left, Callout right)
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
			return Equals((Callout)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Number * 397) ^ (Text?.GetHashCode() ?? 0);
			}
		}

		protected bool Equals(Callout other)
		{
			return Number == other.Number && string.Equals(Text, other.Text);
		}
	}
}