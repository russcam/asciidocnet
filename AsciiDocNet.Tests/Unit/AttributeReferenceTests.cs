using System;
using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public class AttributeReferenceTests : DelimitedPairInlineElementTests<AttributeReference>
	{
		public override string Text { get; } = "attribute-reference";

		public override Tuple<string,string>[] DelimiterPairs { get; } = { Tuple.Create("{", "}") };
	}
}