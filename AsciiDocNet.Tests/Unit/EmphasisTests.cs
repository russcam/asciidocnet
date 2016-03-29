using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class EmphasisTests : DelimitedInlineElementTests<Emphasis>
	{
		public override string[] Delimiters { get; } = { "_", "__" };
	}
}