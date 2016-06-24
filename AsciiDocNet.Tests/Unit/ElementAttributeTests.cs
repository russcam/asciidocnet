using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class ElementAttributeTests
	{
		private Parser Parser { get; } = new Parser();

		[Theory]
		[InlineData("[#idname.rolename]\nparagraph")]
		[InlineData("[id=\"idname\",.rolename]\nparagraph")]
		[InlineData("[id=\"idname\",role=\"rolename\"]\nparagraph")]
		public void ShouldParseAsciiIdElementAttributeAndAsciiRoleElementAttribute(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 2);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<IdAttribute>(firstAttribute);
			var idAttribute = (IdAttribute)firstAttribute;

			Assert.Equal("idname", idAttribute.Value);

			var secondAttribute = elementAttributes[1];

			Assert.IsType<RoleAttribute>(secondAttribute);
			var roleAttribute = (RoleAttribute)secondAttribute;

			Assert.Equal(1, roleAttribute.Values.Length);
			Assert.Equal("rolename", roleAttribute.Values[0]);
		}

		[Fact]
		public void ShouldParseOptionAttribute()
		{
			var document = Document.Parse("[%header%footer%autowidth]\nparagraph");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<OptionsAttribute>(firstAttribute);
			var optionsAttribute = (OptionsAttribute)firstAttribute;

			Assert.Equal(3, optionsAttribute.Values.Length);
			Assert.Equal("header,footer,autowidth", optionsAttribute.Value);
		}

		[Fact]
		public void ShouldParseSingleAsciiElementAttribute()
		{
			var document = Document.Parse("[name]\nparagraph");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<Attribute>(firstAttribute);
			Assert.Equal("name", firstAttribute.Name);
		}

		[Theory]
		[InlineData("[#name]\nparagraph")]
		[InlineData("[id=\"name\"]\nparagraph")]
		public void ShouldParseSingleAsciiIdElementAttribute(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<IdAttribute>(firstAttribute);

			var idAttribute = (IdAttribute)firstAttribute;

			Assert.Equal("id", idAttribute.Name);
			Assert.Equal("name", idAttribute.Value);
		}

		[Fact]
		public void ShouldParseSingleAsciiNamedElementAttribute()
		{
			var document = Document.Parse("[name=\"value\"]\nparagraph");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<NamedAttribute>(firstAttribute);
			var namedAttribute = (NamedAttribute)firstAttribute;

			Assert.Equal("value", namedAttribute.Value);
		}

		[Theory]
		[InlineData("[role=\"value\"]\nparagraph")]
		[InlineData("[.value]\nparagraph")]
		public void ShouldParseSingleAsciiRoleElementAttribute(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<RoleAttribute>(firstAttribute);
			var roleAttribute = (RoleAttribute)firstAttribute;

			Assert.Equal(1, roleAttribute.Values.Length);
			Assert.Equal("value", roleAttribute.Values[0]);
		}

		[Theory]
		[InlineData("[role=\"value1,value2,value3\"]\nparagraph")]
		[InlineData("[.value1.value2.value3]\nparagraph")]
		public void ShouldParseSingleAsciiRoleElementAttributeWithMultipleValues(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<RoleAttribute>(firstAttribute);
			var roleAttribute = (RoleAttribute)firstAttribute;

			Assert.Equal(3, roleAttribute.Values.Length);
			Assert.Equal("value1", roleAttribute.Values[0]);
			Assert.Equal("value2", roleAttribute.Values[1]);
			Assert.Equal("value3", roleAttribute.Values[2]);
		}

		[Fact]
		public void ShouldParseStyleAndIdElementAttributes()
		{
			var document = Document.Parse("[quote#think]\nparagraph");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Quote>(document[0]);
			var quote = (Quote)document[0];

			var elementAttributes = quote.Attributes;

			Assert.True(elementAttributes.Count == 2);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<Attribute>(firstAttribute);
			Assert.Equal("quote", firstAttribute.Name);

			var secondAttribute = elementAttributes[1];

			Assert.IsType<IdAttribute>(secondAttribute);
			var idAttribute = (IdAttribute)secondAttribute;

			Assert.Equal("id", idAttribute.Name);
			Assert.Equal("think", idAttribute.Value);
		}

		[Fact]
		public void ShouldParseCustomAttribute()
		{
			var document = Document.Parse("[source, javascript, method-name=\"fluent\"]\nparagraph");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Source>(document[0]);
			var source = (Source)document[0];

			var elementAttributes = source.Attributes;

			Assert.True(elementAttributes.Count == 3);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<Attribute>(firstAttribute);
			Assert.Equal("source", firstAttribute.Name);

			var secondAttribute = elementAttributes[1];

			Assert.IsType<Attribute>(secondAttribute);
			Assert.Equal("javascript", secondAttribute.Name);

			var thirdAttribute = elementAttributes[2];

			Assert.IsType<NamedAttribute>(thirdAttribute);
			var namedAttribute = (NamedAttribute)thirdAttribute;

			Assert.Equal("method-name", namedAttribute.Name);
			Assert.Equal("fluent", namedAttribute.Value);
		}

		[Theory]
		[InlineData('\'')]
		[InlineData('"')]
		public void ShouldParseQuotedAttribute(char quoteCharacter)
		{
			var value = "'value'";

			if (quoteCharacter == '\'')
			{
				value = "\"value\"";
			}
			var document = Document.Parse($"[name={quoteCharacter}{value}{quoteCharacter}]\nparagraph");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.True(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsType<NamedAttribute>(firstAttribute);
			var namedAttribute = (NamedAttribute)firstAttribute;

			Assert.Equal("name", namedAttribute.Name);
			Assert.Equal(value, namedAttribute.Value);
		}
	}
}