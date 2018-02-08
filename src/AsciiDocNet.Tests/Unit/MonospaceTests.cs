using System.Collections.Generic;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class MonospaceTests : DelimitedContainerInlineElementTests<Monospace, MonospaceTestsDelimiters>
	{
		[Fact]
		public void ShouldOutputCorrectResponse()
		{
			var text = @"When specifying a `format` **and** `extended_bounds`, in order for Elasticsearch to be able to parse 
the serialized ``DateTime``s of `extended_bounds` correctly, the `date_optional_time` format is included 
as part of the `format` value.";

			var document = Document.Parse(text);

			AsciiDocAssert.Equal(text, document);
		}
	}

    public class MonospaceTestsDelimiters : ReusableClassData
	{
		protected override IEnumerable<object[]> Data 
		{ 
			get 
			{ 
				yield return new object[] { "`" };
				yield return new object[] { "``" };
				yield return new object[] { "+" };
				yield return new object[] { "++" };
			}
		}
	}
}