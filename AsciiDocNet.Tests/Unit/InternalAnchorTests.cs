using System;

namespace AsciiDocNet.Tests.Unit
{
	public class InternalAnchorTests : AnchorTests<InternalAnchor>
	{
		public override Tuple<string, string> DelimiterPair { get; } = Tuple.Create("<<", ">>");
	}
}