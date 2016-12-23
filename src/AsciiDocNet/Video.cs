namespace AsciiDocNet
{
    /// <summary>
    /// A video element.
    /// </summary>
    /// <seealso cref="AsciiDocNet.Media" />
    public class Video : Media
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public Video(string path) : base(path)
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
			visitor.VisitVideo(this);
			return visitor;
		}
	}
}