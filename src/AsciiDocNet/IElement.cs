namespace AsciiDocNet
{
    /// <summary>
    /// An AsciiDoc element
    /// </summary>
    public interface IElement
	{
        /// <summary>
        /// Gets or sets the parent element
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        Container Parent { get; set; }

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The visitor</returns>
        TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor;
	}
}