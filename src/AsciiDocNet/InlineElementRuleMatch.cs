namespace AsciiDocNet
{
    /// <summary>
    /// A match for an inline element rule
    /// </summary>
    /// <typeparam name="TInlineElement">The type of the inline element.</typeparam>
    /// <seealso cref="AsciiDocNet.InlineElementRuleMatch" />
    public class InlineElementRuleMatch<TInlineElement> : InlineElementRuleMatch where TInlineElement : IInlineElement
	{
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        public new TInlineElement Element => (TInlineElement)base.Element;

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineElementRuleMatch{TInlineElement}"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">The end index.</param>
        /// <param name="replacement">The replacement string.</param>
        public InlineElementRuleMatch(TInlineElement element, int startIndex, int endIndex, string replacement) 
			: base(element, startIndex, endIndex, replacement)
		{
		}
	}

    /// <summary>
    /// A match for an inline element rule
    /// </summary>
    /// <seealso cref="AsciiDocNet.InlineElementRuleMatch" />
    public class InlineElementRuleMatch
	{
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        public IInlineElement Element { get; }

        /// <summary>
        /// Gets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        public int StartIndex { get; }

        /// <summary>
        /// Gets the end index.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        public int EndIndex { get; }
		
		/// <summary>
		/// Gets the replacement string.
		/// </summary>
		public string Replacement { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineElementRuleMatch"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">The end index.</param>
        /// <param name="replacement">The replacement string.</param>
        public InlineElementRuleMatch(IInlineElement element, int startIndex, int endIndex, string replacement)
		{
			Element = element;
			StartIndex = startIndex;
			EndIndex = endIndex;
			Replacement = replacement;
		}
	}
}