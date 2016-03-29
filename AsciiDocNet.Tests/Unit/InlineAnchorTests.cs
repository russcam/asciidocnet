using System;
using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class InlineAnchorTests : AnchorTests<InlineAnchor>
	{
		public override Tuple<string, string> DelimiterPair { get; } = Tuple.Create("[[", "]]");
	}
}