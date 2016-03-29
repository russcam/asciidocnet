using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class MonospaceTests : DelimitedContainerInlineElementTests<Monospace>
	{
		public override string[] Delimiters { get; } = { "`" };

		[Test]
		public void ShouldOutputCorrectResponse()
		{
			var text = @"When specifying a `format` **and** `extended_bounds`, in order for Elasticsearch to be able to parse 
the serialized ``DateTime``s of `extended_bounds` correctly, the `date_optional_time` format is included 
as part of the `format` value.";

			var document = Document.Parse(text);

			AsciiDocAssert.AreEqual(text, document);
		}
	}
}