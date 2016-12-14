namespace AsciiDocNet
{
	public class Example : Container, IElement, IAttributable
	{
		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public static bool operator ==(Example left, Example right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Example left, Example right)
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
			return Equals((Example)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ (Attributes != null ? Attributes.GetHashCode() : 0);
			}
		}

		protected bool Equals(Example other)
		{
			return base.Equals(other) && Equals(Attributes, other.Attributes);
		}
	}
}