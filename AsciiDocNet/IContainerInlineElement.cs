using System.Collections.Generic;

namespace AsciidocNet
{
	public interface IContainerInlineElement : IInlineElement
	{
		IList<IInlineElement> Elements { get; }

		InlineElementType ContainElementType { get; }
	}
}