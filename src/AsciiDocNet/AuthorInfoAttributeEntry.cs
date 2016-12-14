using System.Linq;

namespace AsciiDocNet
{
	public class AuthorInfoAttributeEntry : AttributeEntry
	{
		public AuthorInfoAttributeEntry(string value) : base("author", value)
		{
			var values = value.Split(' ');

			if (values.Length == 1)
			{
				FirstName = values[0];
			}
			else if (values.Length == 2)
			{
				FirstName = values[0];
				LastName = values[1];
			}
			else
			{
				FirstName = values[0];
				LastName = values[values.Length - 1];
				MiddleName = string.Join(" ", values.Skip(1).Take(values.Length - 2));
			}
		}

		public string FirstName { get; }

		public string LastName { get; }

		public string MiddleName { get; }

		public static bool operator ==(AuthorInfoAttributeEntry left, AuthorInfoAttributeEntry right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(AuthorInfoAttributeEntry left, AuthorInfoAttributeEntry right)
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
			return obj.GetType() == this.GetType() && Equals((AuthorInfoAttributeEntry)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (FirstName?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (LastName?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (MiddleName?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		protected bool Equals(AuthorInfoAttributeEntry other)
		{
			return base.Equals(other) &&
			       string.Equals(FirstName, other.FirstName) &&
			       string.Equals(LastName, other.LastName) &&
			       string.Equals(MiddleName, other.MiddleName);
		}
	}
}