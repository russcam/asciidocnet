namespace AsciiDocNet
{
	public class IdAttribute : NamedAttribute
	{
		public IdAttribute(string value, bool singleQuoted) : base("id", value, singleQuoted)
		{
		}

		public IdAttribute(string value, string reference, bool singleQuoted) : base("id", value, singleQuoted)
		{
			Reference = reference;
		}

		public string Reference { get; set; }

		public static bool operator ==(IdAttribute left, IdAttribute right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(IdAttribute left, IdAttribute right)
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
			return Equals((IdAttribute)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ (Reference?.GetHashCode() ?? 0);
			}
		}

		protected bool Equals(IdAttribute other)
		{
			return base.Equals(other) && string.Equals(Reference, other.Reference);
		}
	}
}