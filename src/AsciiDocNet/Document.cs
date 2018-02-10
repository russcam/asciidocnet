using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// A Asciidoc document
    /// </summary>
    /// <seealso cref="AsciiDocNet.Container" />
    public class Document : Container
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document() : this(null)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="source">The source path of the document</param>
        public Document(string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				var directory = new FileInfo(source).Directory.FullName;
				Attributes.Add(new AttributeEntry("docdir", directory));
			}
			else
			{
				Attributes.Add(new AttributeEntry("docdir", Directory.GetCurrentDirectory()));
			}
		}

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IList<AttributeEntry> Attributes { get; } = new List<AttributeEntry>();

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <value>
        /// The authors.
        /// </value>
        public IList<AuthorInfo> Authors { get; } = new List<AuthorInfo>();

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        public DocType DocType { get; set; } = DocType.Article;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public DocumentTitle Title { get; set; }

        /// <summary>
        /// Loads a document from the specified path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A new instance of <see cref="Document" />
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.AggregateException"></exception>
        public static Document Load(string path)
		{
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException($"{nameof(path)} must not be empty.");
            }

			var reader = new DocumentReader(path);
			var parser = new Parser();
			return parser.Parse(reader);
		}

        /// <summary>
        /// Loads a document from the specified stream
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// A new instance of <see cref="Document" />
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public static Document Load(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

            if (stream.Length == 0)
            {
                throw new ArgumentException($"{nameof(stream)} must not be empty.");
            }

			using (var streamReader = new StreamReader(stream))
			{
				var reader = new DocumentReader(streamReader);
				var parser = new Parser();
				return parser.Parse(reader);
			}
		}

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Document left, Document right)
		{
			return Equals(left, right);
		}

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Document left, Document right)
		{
			return !Equals(left, right);
		}

        /// <summary>
        /// Parses a document from the specified string
        /// </summary>
        /// <param name="text">The string.</param>
        /// <returns>A new instance of <see cref="Document"/></returns>
        public static Document Parse(string text)
		{
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length == 0)
            {
                throw new ArgumentException($"{nameof(text)} must not be empty.");
            }

            using (var stringReader = new StringReader(text))
            {
	            var reader = new DocumentReader(stringReader);
				var parser = new Parser();
				return parser.Parse(reader);
			}
		}

        /// <summary>
        /// Accepts a visitor to visit this document instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitDocument(this);
			return visitor;
		}

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
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
			return obj.GetType() == this.GetType() && Equals((Document)obj);
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
				hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ Authors.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)DocType;
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

        /// <summary>
        /// Determines whether the specified <see cref="Document" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Document other)
		{
			return base.Equals(other) &&
			       Attributes.SequenceEqual(other.Attributes) &&
			       Authors.SequenceEqual(other.Authors) &&
			       DocType == other.DocType &&
			       Equals(Title, other.Title);
		}
	}
}