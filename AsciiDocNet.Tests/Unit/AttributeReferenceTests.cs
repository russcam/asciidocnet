using System;
using System.Collections.Generic;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class AttributeReferenceTests : DelimitedPairInlineElementTests<AttributeReference, AttributeReferenceTestsPairs>
	{
		public override string Text { get; } = "attribute-reference";
	}

	public class AttributeReferenceTestsPairs : ReusableClassData
	{
		protected override IEnumerable<object[]> Data 
		{ 
			get 
			{
				 yield return new object[] { Tuple.Create("{", "}") };
			}
		}
	}
}
