namespace AsciiDocNet
{
    /// <summary>
    /// An image element
    /// </summary>
    /// <example>
    /// image:images/logo.png[Company Logo]
    /// </example>
    /// <example>
    /// image::images/tiger.png["Tiger image",align="left"]
    /// </example>
    /// <seealso cref="AsciiDocNet.Media" />
    public class Image : Media
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public Image(string path) : base(path)
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
			visitor.VisitImage(this);
			return visitor;
		}
	}
}