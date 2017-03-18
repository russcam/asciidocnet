using System;

namespace AsciiDocNet
{
    /// <summary>
    /// An include element (macro) is used to include the contents of a named file into the source document.
    /// </summary>
    /// <seealso cref="AsciiDocNet.IElement" />
    public class Include : IElement
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Include"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">must specify a path</exception>
        public Include(string path)
		{
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (path.Length == 0)
                throw new ArgumentException("must specify a path", nameof(path));

            Path = path;
		}

        /// <summary>
        /// Gets or sets the indent.
        /// </summary>
        /// <value>
        /// The indent.
        /// </value>
        public int? Indent { get; set; }

        /// <summary>
        /// Gets or sets the level offset.
        /// </summary>
        /// <value>
        /// The level offset.
        /// </value>
        public int? LevelOffset { get; set; }

        /// <summary>
        /// Gets or sets the lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public string Lines { get; set; }

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
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public string Tags { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Include left, Include right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Include left, Include right) => !Equals(left, right);

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

			return obj.GetType() == this.GetType() && Equals((Include)obj);
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
				var hashCode = Indent.GetHashCode();
				hashCode = (hashCode * 397) ^ LevelOffset.GetHashCode();
				hashCode = (hashCode * 397) ^ (Lines?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Parent?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Path?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Tags?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// The visitor
        /// </returns>
        public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.VisitInclude(this);
			return visitor;
		}

        /// <summary>
        /// Determines whether the specified <see cref="Include" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Include other) => 
            Indent == other.Indent &&
		    LevelOffset == other.LevelOffset &&
		    string.Equals(Lines, other.Lines) &&
		    string.Equals(Path, other.Path) &&
		    string.Equals(Tags, other.Tags);
	}
}