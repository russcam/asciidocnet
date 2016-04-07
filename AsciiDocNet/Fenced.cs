namespace AsciiDocNet
{
	public class Fenced : IElement, IAttributable, IText
	{
		public Fenced()
		{
		}

		public Fenced(string text)
		{
			Text = text;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Fenced left, Fenced right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Fenced left, Fenced right)
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
			return Equals((Fenced)obj);
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

		protected bool Equals(Fenced other)
		{
			return Equals(Attributes, other.Attributes) && string.Equals(Text, other.Text);
		}
	}
}