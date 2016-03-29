using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class AttributeEntryTests
	{
		[Test]
		[TestCase(":!name:")]
		[TestCase(":name!:")]
		public void ShouldParseAsciiUnsetAttributeEntry(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Attributes.Count == 2);
			Assert.IsInstanceOf<UnsetAttributeEntry>(document.Attributes[1]);
			Assert.AreEqual("name", document.Attributes[1].Name);
			Assert.IsNull(document.Attributes[1].Value);
		}

		[Test]
		[TestCase(":name: value")]
		[TestCase(":name:  value  ")]
		[TestCase("  :name:  value  ")]
		public void ShouldParseAsciiAttributeEntry(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Attributes.Count == 2);
			Assert.AreEqual("name", document.Attributes[1].Name);
			Assert.AreEqual("value", document.Attributes[1].Value);
		}

		[Test]
		[TestCase(":author: firstName")]
		public void ShouldParseAsciiAuthorInfoAttributeEntry(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Attributes.Count == 2);
			Assert.IsInstanceOf<AuthorInfoAttributeEntry>(document.Attributes[1]);
			Assert.AreEqual("author", document.Attributes[1].Name);
			Assert.AreEqual("firstName", document.Attributes[1].Value);
		}
	}
}