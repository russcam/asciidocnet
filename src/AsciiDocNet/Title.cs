namespace AsciiDocNet
{
    /// <summary>
    /// A title for a block.
    /// <para></para>
    /// You can assign a title to any paragraph, list, delimited block, or block macro. 
    /// In most cases, the title is displayed immediately above the content. 
    /// If the content is a figure or image, the title is displayed below the content.
    /// </summary>
    /// <example>
    /// .TO DO list
    /// - Learn the AsciiDoc syntax
    /// - Write my document
    /// </example>
    public class Title
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Title"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public Title(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Title left, Title right) => Equals(left, right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Title left, Title right) => !Equals(left, right);

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
        {
            visitor.VisitTitle(this);
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
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == this.GetType() && Equals((Title)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => Text?.GetHashCode() ?? 0;

        /// <summary>
        /// Determines whether the specified <see cref="Title" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if equal; otherwise, false</returns>
        protected bool Equals(Title other) => string.Equals(Text, other.Text);
    }
}