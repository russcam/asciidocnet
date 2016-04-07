namespace AsciiDocNet
{
	public class Admonition : Container, IElement, IAttributable
	{
		public Admonition(AdmonitionStyle style)
		{
			Style = style;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public AdmonitionStyle Style { get; set; }

		public static bool operator ==(Admonition left, Admonition right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Admonition left, Admonition right)
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
			return Equals((Admonition)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (Attributes?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (int)Style;
				return hashCode;
			}
		}

		protected bool Equals(Admonition other)
		{
			return base.Equals(other) &&
			       Equals(Attributes, other.Attributes) &&
			       Style == other.Style;
		}
	}
}