using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class AttributeEntryTests
	{
		[Theory]
		[InlineData(":!name:")]
		[InlineData(":name!:")]
		public void ShouldParseAsciiUnsetAttributeEntry(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Attributes.Count == 2);
			Assert.IsType<UnsetAttributeEntry>(document.Attributes[1]);
			Assert.Equal("name", document.Attributes[1].Name);
			Assert.Null(document.Attributes[1].Value);
		}

		[Theory]
		[InlineData(":name: value")]
		[InlineData(":name:  value  ")]
		[InlineData("  :name:  value  ")]
		public void ShouldParseAsciiAttributeEntry(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Attributes.Count == 2);
			Assert.Equal("name", document.Attributes[1].Name);
			Assert.Equal("value", document.Attributes[1].Value);
		}

		[Theory]
		[InlineData(":author: firstName")]
		public void ShouldParseAsciiAuthorInfoAttributeEntry(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Attributes.Count == 2);
			Assert.IsType<AuthorInfoAttributeEntry>(document.Attributes[1]);
			Assert.Equal("author", document.Attributes[1].Name);
			Assert.Equal("firstName", document.Attributes[1].Value);
		}
	}
}