namespace AsciiDocNet
{
    /// <summary>
    /// Author Information
    /// </summary>
    public class AuthorInfo
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(MiddleName) && !string.IsNullOrEmpty(LastName))
                    return $"{FirstName} {MiddleName} {LastName}";
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                    return $"{FirstName} {LastName}";
                if (!string.IsNullOrEmpty(FirstName))
                    return FirstName;
                if (!string.IsNullOrEmpty(LastName))
                    return LastName;

                return null;
            }
        }

        /// <summary>
        /// Gets the initials.
        /// </summary>
        /// <value>
        /// The initials.
        /// </value>
        public string Initials
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(MiddleName) && !string.IsNullOrEmpty(LastName))
                    return $"{FirstName[0]} {MiddleName[0]} {LastName[0]}";
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                    return $"{FirstName[0]} {LastName[0]}";

                return FirstName[0].ToString();
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string MiddleName { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(AuthorInfo left, AuthorInfo right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(AuthorInfo left, AuthorInfo right) => !Equals(left, right);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == this.GetType() && Equals((AuthorInfo)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
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

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The visitor</returns>
        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
        {
            visitor.VisitAuthorInfo(this);
            return visitor;
        }

        /// <summary>
        /// Determines whether the specified <see cref="AuthorInfo" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(AuthorInfo other) =>
            string.Equals(FirstName, other.FirstName) &&
            string.Equals(MiddleName, other.MiddleName) &&
            string.Equals(LastName, other.LastName) &&
            string.Equals(Email, other.Email);
    }
}