using System;

namespace AsciiDocNet.Tests.Unit
{
	public class InlineAnchorTests : AnchorTests<InlineAnchor>
	{
		public override Tuple<string, string> DelimiterPair { get; } = Tuple.Create("[[", "]]");
	}
}