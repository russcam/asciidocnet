using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// Author Information in an Attribute Entry
    /// </summary>
    /// <seealso cref="AsciiDocNet.AttributeEntry" />
    public class AuthorInfoAttributeEntry : AttributeEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorInfoAttributeEntry"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public AuthorInfoAttributeEntry(string value) : base("author", value)
        {
            var values = value.Split(' ');

            if (values.Length == 1)
                FirstName = values[0];
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

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; }

        /// <summary>
        /// Gets the middle name.
        /// </summary>
        /// <value>
        /// The middle name.
        /// </value>
        public string MiddleName { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(AuthorInfoAttributeEntry left, AuthorInfoAttributeEntry right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(AuthorInfoAttributeEntry left, AuthorInfoAttributeEntry right) => !Equals(left, right);

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
            return obj.GetType() == this.GetType() && Equals((AuthorInfoAttributeEntry)obj);
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
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (FirstName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (LastName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (MiddleName?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="AuthorInfoAttributeEntry" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(AuthorInfoAttributeEntry other) => 
            base.Equals(other) &&
            string.Equals(FirstName, other.FirstName) &&
            string.Equals(LastName, other.LastName) &&
            string.Equals(MiddleName, other.MiddleName);
    }
}