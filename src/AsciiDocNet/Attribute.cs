using System;

namespace AsciiDocNet
{
    /// <summary>
    /// An attribute with an <see cref="AttributeList"/>
    /// </summary>
    public class Attribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Attribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="skipCheck">if set to <c>true</c> [skip check].</param>
        /// <exception cref="System.ArgumentException"></exception>
        internal Attribute(string name, bool skipCheck)
		{
		    if (!skipCheck && !IsValidName(name))
		        throw new ArgumentException($"{name} is not a valid attribute name");

		    Name = name;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Attribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Attribute(string name) : this(name, false) { }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Determines whether [is valid name] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool IsValidName(string name) => PatternMatcher.AttributeValidName.IsMatch(name);

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Attribute left, Attribute right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Attribute left, Attribute right) => !Equals(left, right);

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.VisitAttribute(this);
			return visitor;
		}

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
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return obj.GetType() == this.GetType() && Equals((Attribute)obj);
		}

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => Name?.GetHashCode() ?? 0;

        /// <summary>
        /// Determines whether the specified <see cref="Attribute" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Attribute other) => string.Equals(Name, other.Name);
	}
}