namespace AsciiDocNet
{
	public class Emphasis : IInlineElement, IText, IAttributable
	{
		public Emphasis(string text, bool doubleDelimited = false)
		{
			Text = text;
			DoubleDelimited = doubleDelimited;
		}

		public Emphasis()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public bool DoubleDelimited { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Emphasis left, Emphasis right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Emphasis left, Emphasis right)
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
			return Equals((Emphasis)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ DoubleDelimited.GetHashCode();
				hashCode = (hashCode * 397) ^ (Text?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Emphasis other)
		{
			return Equals(Attributes, other.Attributes) && DoubleDelimited == other.DoubleDelimited && string.Equals(Text, other.Text);
		}
	}
}