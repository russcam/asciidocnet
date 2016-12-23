namespace AsciiDocNet
{
    /// <summary>
    /// A table element
    /// </summary>
    /// <example>
    /// |=== 
    /// 
    /// | Cell in column 1, row 1 | Cell in column 2, row 1  
    /// 
    /// | Cell in column 1, row 2 | Cell in column 2, row 2
    /// 
    /// | Cell in column 1, row 3 | Cell in column 2, row 3
    /// 
    /// |=== 
    /// </example>
    /// <seealso cref="AsciiDocNet.Container" />
    /// <seealso cref="AsciiDocNet.IElement" />
    /// <seealso cref="AsciiDocNet.IAttributable" />
    public class Table : Container, IElement, IAttributable
	{
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public AttributeList Attributes { get; } = new AttributeList();

        /// <summary>
        /// Gets or sets the parent element
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Container Parent { get; set; }

        /// <summary>
        /// Accepts a visitor to visit this element instance
        /// </summary>
        /// <typeparam name="TVisitor">The type of the visitor.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns></returns>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.VisitTable(this);
			return visitor;
		}
	}
}