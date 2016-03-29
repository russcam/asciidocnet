namespace AsciiDocNet
{
	public class NamedAttribute : Attribute
	{
		public NamedAttribute(string name, string value, bool singleQuoted) : base(name)
		{
			Value = value;
			SingleQuoted = singleQuoted;
		}

		public bool SingleQuoted { get; set; }

		public string Value { get; set; }

		public static bool operator ==(NamedAttribute left, NamedAttribute right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(NamedAttribute left, NamedAttribute right)
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
			return Equals((NamedAttribute)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ (Value?.GetHashCode() ?? 0);
			}
		}

		protected bool Equals(NamedAttribute other)
		{
			return base.Equals(other) && string.Equals(Value, other.Value);
		}
	}
}