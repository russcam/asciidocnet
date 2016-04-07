using System.Collections.Generic;

namespace AsciiDocNet
{
	// TODO: This should be a collection, much like Container
	public interface IContainerInlineElement : IInlineElement
	{
		IList<IInlineElement> Elements { get; }

		InlineElementType ContainElementType { get; }
	}
}