using System.Collections.Generic;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class MarkTests : DelimitedContainerInlineElementTests<Mark, MarkTestsDelimiters>
	{
		[Theory]
		[MemberData(nameof(ClassData))]
		public void ShouldParseMarkWithStyle(string delimiter)
		{
			var text = $"[green]{delimiter}{Paragraph}{delimiter}";
			var document = Document.Parse(text);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.Equal(1, paragraph.Count);
			Assert.IsType<Mark>(paragraph[0]);

			var element = (Mark)paragraph[0];
			Assert.Equal(Paragraph, ((TextLiteral)element[0]).Text);
			Assert.Equal("green", ((RoleAttribute)element.Attributes[0]).Value);
		}

		[Theory]
		[MemberData(nameof(ClassData))]
		public void ShouldNotParseMarkFromSingleDelimiter(string delimiter)
		{
			var text = $"{Paragraph}{delimiter}";
			var document = Document.Parse(text);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.Equal(1, paragraph.Count);
			Assert.IsType<TextLiteral>(paragraph[0]);

			var element = (TextLiteral)paragraph[0];
			Assert.Equal(Paragraph + delimiter, element.Text);
		}
	}

    public class MarkTestsDelimiters : ReusableClassData
	{
		protected override IEnumerable<object[]> Data 
		{ 
			get 
			{ 
				yield return new object[] { "#" };
				yield return new object[] { "##" };
			}
		}
	}
}