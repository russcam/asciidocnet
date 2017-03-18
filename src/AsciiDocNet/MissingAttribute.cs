namespace AsciiDocNet
{
    /// <summary>
    /// Controls how missing references are handled.
    /// </summary>
    public enum MissingAttribute
	{
        /// <summary>
        /// leave the reference in place (default setting)
        /// </summary>
        Skip,
        /// <summary>
        /// drop the reference, but not the line
        /// </summary>
        Drop,
        /// <summary>
        /// drop the line on which the reference occurs (compliant behavior)
        /// </summary>
        DropLine,
        /// <summary>
        /// print a warning about the missing attribute
        /// </summary>
        Warn
    }
}