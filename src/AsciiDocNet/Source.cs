namespace AsciiDocNet
{
    /// <summary>
    /// A source element.
    /// <para></para>
    /// Source code or keyboard input to be displayed as entered
    /// </summary>
    /// <example>
    /// [source, c#]
    /// ----
    /// public class Foo {}
    /// ----
    /// </example>
    /// <seealso cref="AsciiDocNet.Listing" />
    public class Source : Listing
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Source"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public Source(string text)
		{
			Text = text;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Source"/> class.
        /// </summary>
        public Source()
		{
		}

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitSource(this);
			return visitor;
		}
	}
}