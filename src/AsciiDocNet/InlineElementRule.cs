using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    /// <summary>
    /// An inline element rule
    /// </summary>
    public class InlineElementRule
    {
        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>
        /// The type of the element.
        /// </value>
        public InlineElementType ElementType { get; }

        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <value>
        /// The regex.
        /// </value>
        public Regex Regex { get; }

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        /// <value>
        /// The constraint.
        /// </value>
        public InlineElementConstraint Constraint { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineElementRule"/> class.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="regex">The regex.</param>
        /// <param name="constraint">The constraint.</param>
        public InlineElementRule(InlineElementType elementType, Regex regex, InlineElementConstraint constraint)
        {
            ElementType = elementType;
            Regex = regex;
            Constraint = constraint;
        }
    }
}