using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class SectionTitleTests
	{
		[Theory]
		[InlineData(2, '#')]
		[InlineData(3, '#')]
		[InlineData(4, '#')]
		[InlineData(5, '#')]
		[InlineData(6, '#')]
		[InlineData(2, '=')]
		[InlineData(3, '=')]
		[InlineData(4, '=')]
		[InlineData(5, '=')]
		[InlineData(6, '=')]
		public void CanParseTitle(int level, char marker)
		{
			var title = $"{new string(marker, level)} This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.IsType<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.Equal("This is the title", text.Text);
			Assert.Equal(level, sectionTitle.Level);
		}

		[Fact]
		public void CanParseDiscreteTitle()
		{
			var title = $"[discrete]\r\n== This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.True(sectionTitle.IsDiscrete);

			Assert.IsType<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.Equal("This is the title", text.Text);
			AsciiDocAssert.Equal(title, document);
		}

		[Fact]
		public void CanParseFloatingTitle()
		{
			var title = $"[float]\r\n== This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.True(sectionTitle.IsFloating);

			Assert.IsType<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.Equal("This is the title", text.Text);
			AsciiDocAssert.Equal(title, document);
		}

		[Fact]
		public void CanParseFloatingTitleWithAttributes()
		{
			var title = $"[[title-anchor]]\r\n[float]\r\n[attribute1,attribute2]\r\n== This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.True(sectionTitle.IsFloating);

			Assert.IsType<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.Equal("This is the title", text.Text);
			AsciiDocAssert.Equal(title, document);
		}
	}
}