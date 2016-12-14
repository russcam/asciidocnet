using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class RomanNumeralsTest
	{
		public static IEnumerable<object[]> TestCases => Enumerable.Range(1, 3046).Select(i => new object[] { i });

		[Theory]
		[MemberData(nameof(TestCases))]
		public void ShouldConvertToRomanNumeralAndBack(int input)
		{
			var numeral = RomanNumerals.ToNumeral(input);
			var converted = RomanNumerals.ToInt(numeral);
			Assert.Equal(converted, input);
		}
	}
}
