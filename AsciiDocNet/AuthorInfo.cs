namespace AsciidocNet
{
	public class AuthorInfo
	{
		public string Email { get; set; }

		public string FirstName { get; set; }

		public string FullName
		{
			get
			{
				if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(MiddleName) && !string.IsNullOrEmpty(LastName))
				{
					return $"{FirstName} {MiddleName} {LastName}";
				}
				if (!string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
				{
					return $"{FirstName} {LastName}";
				}
				if (!string.IsNullOrEmpty(FirstName))
				{
					return $"{FirstName}";
				}
				if (!string.IsNullOrEmpty(LastName))
				{
					return $"{LastName}";
				}

				return null;
			}
		}

		public string Initials
		{
			get
			{
				if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(MiddleName) && !string.IsNullOrEmpty(LastName))
				{
					return $"{FirstName[0]} {MiddleName[0]} {LastName[0]}";
				}

				if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
				{
					return $"{FirstName[0]} {LastName[0]}";
				}

				return FirstName[0].ToString();
			}
		}

		public string LastName { get; set; }

		public string MiddleName { get; set; }

		public static bool operator ==(AuthorInfo left, AuthorInfo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(AuthorInfo left, AuthorInfo right)
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
			return Equals((AuthorInfo)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = FirstName?.GetHashCode() ?? 0;
				hashCode = (hashCode * 397) ^ (MiddleName?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (LastName?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Email?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(AuthorInfo other)
		{
			return string.Equals(FirstName, other.FirstName) &&
			       string.Equals(MiddleName, other.MiddleName) &&
			       string.Equals(LastName, other.LastName) &&
			       string.Equals(Email, other.Email);
		}
	}
}