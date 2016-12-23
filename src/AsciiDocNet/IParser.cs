namespace AsciiDocNet
{
    /// <summary>
    /// Parses text from an <see cref="IDocumentReader"/> into an <see cref="Document"/>
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Parses text from <see cref="IDocumentReader"/> into an <see cref="Document"/>
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An new instance of <see cref="Document"/></returns>
        Document Parse(IDocumentReader reader);
    }
}