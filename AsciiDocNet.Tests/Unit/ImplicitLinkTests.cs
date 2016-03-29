using System.Collections.Generic;
using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public class ImplicitLinkTests
	{
		public IEnumerable<string> Protocols => new[]
		{
			"http",
			"https",
			"ftp",
			"file",
			"irc"
		};

		public IEnumerable<TestCaseData> TestCases
		{
			get
			{
				foreach (var protocol in Protocols)
				{
					yield return new TestCaseData(protocol, $"{protocol}://example.com[link]", "link", 1, 0);
					yield return new TestCaseData(protocol, $"{protocol}://example.com[]", string.Empty, 1, 0);
					yield return new TestCaseData(protocol, $"This is a paragraph with a {protocol}://example.com[link] in", "link", 3, 1);
				}
			}
		}

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void ShouldParseImplicitAnchor(string protocol, string asciidoc, string linkText, int count, int index)
		{
			var document = Document.Parse(asciidoc);

			Assert.IsNotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.IsTrue(paragraph.Elements.Count == count);

			Assert.IsInstanceOf<Link>(paragraph.Elements[index]);

			var link = (Link)paragraph.Elements[index];

			Assert.AreEqual($"{protocol}://example.com", link.Href);
			Assert.AreEqual(linkText, link.Text);

			AsciiDocAssert.AreEqual(asciidoc, document);
		}

		[Test]
		public void ShouldNotParseImplicitLinkInsideMonospace()
		{
			var text = "This is a paragraph with a `http://example.com` in";
			var document = Document.Parse(text);

			Assert.IsNotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.IsTrue(paragraph.Elements.Count == 3);
			Assert.IsInstanceOf<Monospace>(paragraph.Elements[1]);

			AsciiDocAssert.AreEqual(text, document);
		}
	}
}