namespace AsciiDocNet
{
    /// <summary>
    /// Controls how expressions that undefine an attribute are handled
    /// </summary>
    public enum UndefinedAttribute
	{
        /// <summary>
        /// substitute the expression with an empty string after processing it
        /// </summary>
        Drop,
        /// <summary>
        /// drop the line that contains this expression (default setting and compliant behavior).
        /// </summary>
        DropLine
    }
}