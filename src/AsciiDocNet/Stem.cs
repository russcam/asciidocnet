namespace AsciiDocNet
{
	public class Stem : IElement, IText, IAttributable
	{
		public Stem(string text)
		{
			Text = text;
		}

		public Stem()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Stem left, Stem right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Stem left, Stem right)
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
			return Equals((Stem)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Attributes.GetHashCode() * 397) ^ (Text?.GetHashCode() ?? 0);
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Stem other)
		{
			return Equals(Attributes, other.Attributes) && string.Equals(Text, other.Text);
		}
	}
}