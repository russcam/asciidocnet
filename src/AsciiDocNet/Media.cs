using System;

namespace AsciiDocNet
{
    /// <summary>
    /// A media element.
    /// </summary>
    /// <seealso cref="AsciiDocNet.IElement" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public abstract class Media : IElement, IAttributable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Media"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">must specify a path</exception>
        protected Media(string path)
		{
		    if (path == null)
		        throw new ArgumentNullException(nameof(path));
		    if (path.Length == 0)
		        throw new ArgumentException("must specify a path", nameof(path));

		    Path = path;
		}

        /// <summary>
        /// Gets or sets the align.
        /// </summary>
        /// <value>
        /// The align.
        /// </value>
        public string Align { get; set; }

        /// <summary>
        /// Gets or sets the alternate text.
        /// </summary>
        /// <value>
        /// The alternate text.
        /// </value>
        public string AlternateText { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public AttributeList Attributes { get; } = new AttributeList();

        /// <summary>
        /// Gets or sets the float.
        /// </summary>
        /// <value>
        /// The float.
        /// </value>
        public string Float { get; set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int? Height { get; private set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the parent element
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Container Parent { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int? Width { get; private set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Media left, Media right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Media left, Media right) => !Equals(left, right);

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// The visitor
        /// </returns>
        public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.VisitMedia(this);
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
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((Media)obj);
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
				var hashCode = Align?.GetHashCode() ?? 0;
				hashCode = (hashCode * 397) ^ (AlternateText?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Attributes?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Float?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ Height.GetHashCode();
				hashCode = (hashCode * 397) ^ (Link?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Path?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Role?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ Width.GetHashCode();
				return hashCode;
			}
		}

        /// <summary>
        /// Sets the height of the width and.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SetWidthAndHeight(int width, int height)
		{
			Width = width;
			Height = height;
		}

        /// <summary>
        /// Determines whether the specified <see cref="Media" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Media other) => 
            string.Equals(Align, other.Align) &&
		    string.Equals(AlternateText, other.AlternateText) &&
		    Equals(Attributes, other.Attributes) &&
		    string.Equals(Float, other.Float) &&
		    Height == other.Height &&
		    Width == other.Width &&
		    string.Equals(Link, other.Link) &&
		    string.Equals(Path, other.Path) &&
		    string.Equals(Role, other.Role) &&
		    string.Equals(Title, other.Title);
	}
}