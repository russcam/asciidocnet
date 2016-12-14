namespace AsciiDocNet
{
	/// <summary>
	///     An attribute set in the header of the document
	/// </summary>
	public class AttributeEntry : Attribute, IElement
	{
		public AttributeEntry(string name, string value) : base(name)
		{
			Value = value;
		}

		public Container Parent { get; set; }

		public string Value { get; set; }

		public static bool operator ==(AttributeEntry left, AttributeEntry right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(AttributeEntry left, AttributeEntry right)
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
			return Equals((AttributeEntry)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (Value?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		protected bool Equals(AttributeEntry other)
		{
			return base.Equals(other) &&
			       string.Equals(Value, other.Value);
		}
	}
}