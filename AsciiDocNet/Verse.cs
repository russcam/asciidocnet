namespace AsciiDocNet
{
	public class Verse : Container, IElement, IAttributable
	{
		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public static bool operator ==(Verse left, Verse right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Verse left, Verse right)
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
			return Equals((Verse)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ Attributes.GetHashCode();
			}
		}

		protected bool Equals(Verse other)
		{
			return base.Equals(other) && Attributes.Equals(other.Attributes);
		}
	}
}