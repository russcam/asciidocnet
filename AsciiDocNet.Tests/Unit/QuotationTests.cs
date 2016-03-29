using System;
using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class QuotationTests : DelimitedPairContainerInlineElementTests<Quotation>
	{
		public override Tuple<string,string>[] DelimiterPairs { get; } = { Tuple.Create("\"`", "`\""), Tuple.Create("'`", "`'") };
	}
}