using System.Collections.Generic;

namespace AsciiDocNet
{
	public interface IContainerInlineElement : IInlineElement
	{
		IList<IInlineElement> Elements { get; }

		InlineElementType ContainElementType { get; }
	}
}