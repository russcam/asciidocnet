namespace AsciiDocNet
{
    /// <summary>
    /// An <see cref="AttributeEntry" /> that has been unset
    /// </summary>
    /// <seealso cref="AsciiDocNet.AttributeEntry" />
    public class UnsetAttributeEntry : AttributeEntry
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsetAttributeEntry"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public UnsetAttributeEntry(string name) : base(name, null)
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
			visitor.VisitUnsetAttributeEntry(this);
			return visitor;
		}
	}
}