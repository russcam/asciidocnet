namespace AsciiDocNet
{
	/// <summary>
	/// Has attributes
	/// </summary>
	public interface IAttributable
	{
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        AttributeList Attributes { get; }
	}
}