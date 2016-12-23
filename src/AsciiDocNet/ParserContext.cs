using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    /// <summary>
    /// Provides context for the parser
    /// </summary>
    internal class ParserContext
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserContext"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="regex">The regex.</param>
        public ParserContext(Container parent, Regex regex)
		{
			Parent = parent;
			Regex = regex;
		}

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Container Parent { get; }

        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <value>
        /// The regex.
        /// </value>
        public Regex Regex { get; }

        /// <summary>
        /// Determines whether the specified input is match
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public bool IsMatch(string input)
		{
			return Regex?.IsMatch(input) ?? false;
		}
	}
}