namespace AsciiDocNet
{
    /// <summary>
    /// An internal anchor
    /// </summary>
    /// <example>
    /// &lt;&lt;anchor, anchor text&gt;&gt;
    /// </example>
    /// <seealso cref="AsciiDocNet.InlineAnchor" />
    public class InternalAnchor : InlineAnchor
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalAnchor"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="xRefLabel">The x reference label.</param>
        public InternalAnchor(string id, string xRefLabel) : base(id, xRefLabel)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalAnchor"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public InternalAnchor(string id) : base(id)
		{
		}

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>
        /// the visitor
        /// </returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitInternalAnchor(this);
			return visitor;
		}
	}
}