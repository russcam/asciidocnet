namespace AsciiDocNet
{
    /// <summary>
    /// An email attribute entry
    /// </summary>
    /// <seealso cref="AsciiDocNet.AttributeEntry" />
    public class EmailAttributeEntry : AttributeEntry
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAttributeEntry"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public EmailAttributeEntry(string value) : base("email", value)
		{
		}
	}
}