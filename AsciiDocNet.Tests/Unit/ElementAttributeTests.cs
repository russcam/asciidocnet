using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public class ElementAttributeTests
	{
		private Parser Parser { get; } = new Parser();

		[Test]
		[TestCase("[#idname.rolename]\nparagraph")]
		[TestCase("[id=\"idname\",.rolename]\nparagraph")]
		[TestCase("[id=\"idname\",role=\"rolename\"]\nparagraph")]
		public void ShouldParseAsciiIdElementAttributeAndAsciiRoleElementAttribute(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 2);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<IdAttribute>(firstAttribute);
			var idAttribute = (IdAttribute)firstAttribute;

			Assert.AreEqual("idname", idAttribute.Value);

			var secondAttribute = elementAttributes[1];

			Assert.IsInstanceOf<RoleAttribute>(secondAttribute);
			var roleAttribute = (RoleAttribute)secondAttribute;

			Assert.AreEqual(1, roleAttribute.Values.Length);
			Assert.AreEqual("rolename", roleAttribute.Values[0]);
		}

		[Test]
		public void ShouldParseOptionAttribute()
		{
			var document = Document.Parse("[%header%footer%autowidth]\nparagraph");

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<OptionsAttribute>(firstAttribute);
			var optionsAttribute = (OptionsAttribute)firstAttribute;

			Assert.AreEqual(3, optionsAttribute.Values.Length);
			Assert.AreEqual("header,footer,autowidth", optionsAttribute.Value);
		}

		[Test]
		public void ShouldParseSingleAsciiElementAttribute()
		{
			var document = Document.Parse("[name]\nparagraph");

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<Attribute>(firstAttribute);
			Assert.AreEqual("name", firstAttribute.Name);
		}

		[Test]
		[TestCase("[#name]\nparagraph")]
		[TestCase("[id=\"name\"]\nparagraph")]
		public void ShouldParseSingleAsciiIdElementAttribute(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<IdAttribute>(firstAttribute);

			var idAttribute = (IdAttribute)firstAttribute;

			Assert.AreEqual("id", idAttribute.Name);
			Assert.AreEqual("name", idAttribute.Value);
		}

		[Test]
		public void ShouldParseSingleAsciiNamedElementAttribute()
		{
			var document = Document.Parse("[name=\"value\"]\nparagraph");

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<NamedAttribute>(firstAttribute);
			var namedAttribute = (NamedAttribute)firstAttribute;

			Assert.AreEqual("value", namedAttribute.Value);
		}

		[Test]
		[TestCase("[role=\"value\"]\nparagraph")]
		[TestCase("[.value]\nparagraph")]
		public void ShouldParseSingleAsciiRoleElementAttribute(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<RoleAttribute>(firstAttribute);
			var roleAttribute = (RoleAttribute)firstAttribute;

			Assert.AreEqual(1, roleAttribute.Values.Length);
			Assert.AreEqual("value", roleAttribute.Values[0]);
		}

		[Test]
		[TestCase("[role=\"value1,value2,value3\"]\nparagraph")]
		[TestCase("[.value1.value2.value3]\nparagraph")]
		public void ShouldParseSingleAsciiRoleElementAttributeWithMultipleValues(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<RoleAttribute>(firstAttribute);
			var roleAttribute = (RoleAttribute)firstAttribute;

			Assert.AreEqual(3, roleAttribute.Values.Length);
			Assert.AreEqual("value1", roleAttribute.Values[0]);
			Assert.AreEqual("value2", roleAttribute.Values[1]);
			Assert.AreEqual("value3", roleAttribute.Values[2]);
		}

		[Test]
		public void ShouldParseStyleAndIdElementAttributes()
		{
			var document = Document.Parse("[quote#think]\nparagraph");

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Quote>(document[0]);
			var quote = (Quote)document[0];

			var elementAttributes = quote.Attributes;

			Assert.IsTrue(elementAttributes.Count == 2);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<Attribute>(firstAttribute);
			Assert.AreEqual("quote", firstAttribute.Name);

			var secondAttribute = elementAttributes[1];

			Assert.IsInstanceOf<IdAttribute>(secondAttribute);
			var idAttribute = (IdAttribute)secondAttribute;

			Assert.AreEqual("id", idAttribute.Name);
			Assert.AreEqual("think", idAttribute.Value);
		}

		[Test]
		public void ShouldParseCustomAttribute()
		{
			var document = Document.Parse("[source, javascript, method-name=\"fluent\"]\nparagraph");

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Source>(document[0]);
			var source = (Source)document[0];

			var elementAttributes = source.Attributes;

			Assert.IsTrue(elementAttributes.Count == 3);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<Attribute>(firstAttribute);
			Assert.AreEqual("source", firstAttribute.Name);

			var secondAttribute = elementAttributes[1];

			Assert.IsInstanceOf<Attribute>(secondAttribute);
			Assert.AreEqual("javascript", secondAttribute.Name);

			var thirdAttribute = elementAttributes[2];

			Assert.IsInstanceOf<NamedAttribute>(thirdAttribute);
			var namedAttribute = (NamedAttribute)thirdAttribute;

			Assert.AreEqual("method-name", namedAttribute.Name);
			Assert.AreEqual("fluent", namedAttribute.Value);
		}

		[Test]
		[TestCase('\'')]
		[TestCase('"')]
		public void ShouldParseQuotedAttribute(char quoteCharacter)
		{
			var value = "'value'";

			if (quoteCharacter == '\'')
			{
				value = "\"value\"";
			}
			var document = Document.Parse($"[name={quoteCharacter}{value}{quoteCharacter}]\nparagraph");

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Paragraph>(document[0]);
			var paragraph = (Paragraph)document[0];

			var elementAttributes = paragraph.Attributes;

			Assert.IsTrue(elementAttributes.Count == 1);

			var firstAttribute = elementAttributes[0];

			Assert.IsInstanceOf<NamedAttribute>(firstAttribute);
			var namedAttribute = (NamedAttribute)firstAttribute;

			Assert.AreEqual("name", namedAttribute.Name);
			Assert.AreEqual(value, namedAttribute.Value);
		}
	}
}