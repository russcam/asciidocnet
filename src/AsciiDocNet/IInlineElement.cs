namespace AsciiDocNet
{
    /// <summary>
    /// An inline AsciiDoc element
    /// </summary>
    public interface IInlineElement
	{
        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>the visitor</returns>
        TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor;
	}
}