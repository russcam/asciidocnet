using System.Collections.Generic;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class ImplicitLinkTests
	{
		public static IEnumerable<string> Protocols => new[]
		{
			"http",
			"https",
			"ftp",
			"file",
			"irc"
		};

		public static IEnumerable<object[]> TestCases
		{
			get
			{
				foreach (var protocol in Protocols)
				{
					yield return new object[]{ protocol, $"{protocol}://example.com[`link`]", "`link`", 1, 0 };
					yield return new object[]{ protocol, $"{protocol}://example.com[link]", "link", 1, 0 };
					yield return new object[]{ protocol, $"{protocol}://example.com[]", string.Empty, 1, 0 };
					yield return new object[]{ protocol, $"This is a paragraph with a {protocol}://example.com[link] in", "link", 3, 1 };
				}
			}
		}

		[Theory]
		[MemberData(nameof(TestCases))]
		public void ShouldParseImplicitAnchor(string protocol, string asciidoc, string linkText, int count, int index)
		{
			var document = Document.Parse(asciidoc);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.True(paragraph.Count == count);

			Assert.IsType<Link>(paragraph[index]);

			var link = (Link)paragraph[index];

			Assert.Equal($"{protocol}://example.com", link.Href);
			Assert.Equal(linkText, link.Text);

			AsciiDocAssert.Equal(asciidoc, document);
		}

		[Fact]
		public void ShouldNotParseImplicitLinkInsideMonospace()
		{
			var text = "This is a paragraph with a `http://example.com` in";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.True(paragraph.Count == 3);
			Assert.IsType<Monospace>(paragraph[1]);

			AsciiDocAssert.Equal(text, document);
		}

		[Fact]
		public void ShouldParseLink()
		{
			var text = "{ref_current}/certutil.html[`certutil` tool]";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.True(paragraph.Count == 4);
			Assert.IsType<AttributeReference>(paragraph[0]);
			Assert.IsType<TextLiteral>(paragraph[1]);
			Assert.IsType<Monospace>(paragraph[2]);
			Assert.IsType<TextLiteral>(paragraph[3]);

			AsciiDocAssert.Equal(text, document);
		}
	}
}
