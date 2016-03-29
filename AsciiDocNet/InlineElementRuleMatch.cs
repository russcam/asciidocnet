namespace AsciidocNet
{
	public class InlineElementRuleMatch<TInlineElement> : InlineElementRuleMatch where TInlineElement : IInlineElement
	{
		public new TInlineElement Element => (TInlineElement)base.Element;

		public InlineElementRuleMatch(TInlineElement element, int startIndex, int endIndex) 
			: base(element, startIndex, endIndex)
		{
		}
	}

	public class InlineElementRuleMatch
	{
		public IInlineElement Element { get; }

		public int StartIndex { get; }

		public int EndIndex { get; }

		public InlineElementRuleMatch(IInlineElement element, int startIndex, int endIndex)
		{
			Element = element;
			StartIndex = startIndex;
			EndIndex = endIndex;
		}
	}
}