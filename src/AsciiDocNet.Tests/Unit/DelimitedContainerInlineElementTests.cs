using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class DelimitedContainerInlineElementTests<TElement, TDelimiters> : ClassDataBase<TDelimiters>
		where TElement : InlineContainer
		where TDelimiters : ReusableClassData, new()
	{
		protected const string Paragraph = "This is a paragraph";

		[Theory]
		[MemberData(nameof(ClassData))]
		public void ShouldParseInlineElement(string delimiter)
		{
			var text = $"{delimiter}{Paragraph}{delimiter}";
			var document = Document.Parse(text);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.Equal(1, paragraph.Count);
			Assert.IsType<TElement>(paragraph[0]);

			var element = (TElement)paragraph[0];
			Assert.Equal(Paragraph, ((TextLiteral)element[0]).Text);
		}

		[Theory]
		[MemberData(nameof(ClassData))]
		public void ShouldParseInlineElementSurroundedBySpaces(string delimiter)
		{
			var text = $"    {delimiter}{Paragraph}{delimiter}    ";
			var document = Document.Parse(text);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.Equal(3, paragraph.Count);
			Assert.IsType<TElement>(paragraph[1]);

			var element = (TElement)paragraph[1];
			Assert.Equal(Paragraph, ((TextLiteral)element[0]).Text);
		}
	}
}
