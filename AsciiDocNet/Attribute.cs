using System;

namespace AsciiDocNet
{
	public class Attribute
	{
		public Attribute(string name)
		{
			if (!IsValidName(name))
			{
				throw new ArgumentException($"{name} is not a valid attribute name");
			}

			Name = name;
		}

		public string Name { get; set; }

		public static bool IsValidName(string name)
		{
			return PatternMatcher.AttributeValidName.IsMatch(name);
		}

		public static bool operator ==(Attribute left, Attribute right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Attribute left, Attribute right)
		{
			return !Equals(left, right);
		}

		public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
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
			return Equals((Attribute)obj);
		}

		public override int GetHashCode()
		{
			return Name?.GetHashCode() ?? 0;
		}

		protected bool Equals(Attribute other)
		{
			return string.Equals(Name, other.Name);
		}
	}
}