namespace AsciiDocNet
{
	public class Pass : IElement, IText, IAttributable
	{
		public Pass(string text)
		{
			Text = text;
		}

		public Pass()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Pass left, Pass right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Pass left, Pass right)
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
			return Equals((Pass)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Attributes.GetHashCode() * 397) ^ Text.GetHashCode();
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Pass other)
		{
			return Attributes.Equals(other.Attributes) && string.Equals(Text, other.Text);
		}
	}
}