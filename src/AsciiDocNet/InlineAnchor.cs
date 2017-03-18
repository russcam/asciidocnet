namespace AsciiDocNet
{
    /// <summary>
    /// An inline anchor element
    /// </summary>
    /// <example>
    /// [[bookmark-a]]Inline anchors make arbitrary content referenceable.
    /// </example>
    /// <example>
    /// * Fist item
    /// * [[step2]]Second item
    /// * Third item
    /// </example>
    /// <seealso cref="AsciiDocNet.Anchor" />
    /// <seealso cref="AsciiDocNet.IInlineElement" />
    public class InlineAnchor : Anchor, IInlineElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InlineAnchor"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="xRefLabel">The x reference label.</param>
        public InlineAnchor(string id, string xRefLabel) : base(id, xRefLabel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineAnchor"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public InlineAnchor(string id) : base(id)
        {
        }

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>the visitor</returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitInlineAnchor(this);
            return visitor;
        }
    }
}