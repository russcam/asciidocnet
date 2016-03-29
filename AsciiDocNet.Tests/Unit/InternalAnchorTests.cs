using System;
using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public class InternalAnchorTests : AnchorTests<InternalAnchor>
	{
		public override Tuple<string, string> DelimiterPair { get; } = Tuple.Create("<<", ">>");
	}
}