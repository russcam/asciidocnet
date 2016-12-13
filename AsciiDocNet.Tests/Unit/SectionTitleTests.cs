using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class SectionTitleTests
	{
		[Test]
		[TestCase(2, '#')]
		[TestCase(3, '#')]
		[TestCase(4, '#')]
		[TestCase(5, '#')]
		[TestCase(6, '#')]
		[TestCase(2, '=')]
		[TestCase(3, '=')]
		[TestCase(4, '=')]
		[TestCase(5, '=')]
		[TestCase(6, '=')]
		public void CanParseTitle(int level, char marker)
		{
			var title = $"{new string(marker, level)} This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.IsInstanceOf<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.AreEqual("This is the title", text.Text);
			Assert.AreEqual(level, sectionTitle.Level);
		}

		[Test]
		public void CanParseDiscreteTitle()
		{
			var title = $"[discrete]\r\n== This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.IsTrue(sectionTitle.IsDiscrete);

			Assert.IsInstanceOf<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.AreEqual("This is the title", text.Text);
			AsciiDocAssert.AreEqual(title, document);
		}

		[Test]
		public void CanParseFloatingTitle()
		{
			var title = $"[float]\r\n== This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.IsTrue(sectionTitle.IsFloating);

			Assert.IsInstanceOf<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.AreEqual("This is the title", text.Text);
			AsciiDocAssert.AreEqual(title, document);
		}

		[Test]
		public void CanParseFloatingTitleWithAttributes()
		{
			var title = $"[float]\r\n[[title-anchor]]\r\n[attribute1,attribute2]\r\n== This is the title";

			var document = Document.Parse(title);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<SectionTitle>(document[0]);

			var sectionTitle = (SectionTitle)document[0];

			Assert.IsTrue(sectionTitle.IsFloating);

			Assert.IsInstanceOf<TextLiteral>(sectionTitle[0]);
			var text = (TextLiteral)sectionTitle[0];

			Assert.AreEqual("This is the title", text.Text);
			AsciiDocAssert.AreEqual(title, document);
		}
	}
}