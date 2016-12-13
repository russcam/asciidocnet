using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class QuoteParagraphTests
	{
		[Test]
		public void ShouldParse()
		{
			var text = @".After landing the cloaked Klingon bird of prey in Golden Gate park:
[quote, Captain James T. Kirk, Star Trek IV: The Voyage Home]     
Everybody remember where we parked.";

			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<Quote>(document[0]);
			var quote = (Quote)document[0];

			Assert.IsTrue(quote.Count == 1);
			Assert.IsInstanceOf<Paragraph>(quote[0]);		
			Assert.IsTrue(quote.Attributes.Count == 3);
			Assert.AreEqual("After landing the cloaked Klingon bird of prey in Golden Gate park:", quote.Attributes.Title.Text);
		}

		[Test]
		public void ShouldParseWithFloat()
		{
			var text = @"[[captain-quote]]
[quote, Captain James T. Kirk, Star Trek IV: The Voyage Home]
.After landing the cloaked Klingon bird of prey in Golden Gate park:
Everybody remember where we parked.";

			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<Quote>(document[0]);
			var quote = (Quote)document[0];

			Assert.IsTrue(quote.Count == 1);
			Assert.IsInstanceOf<Paragraph>(quote[0]);
			Assert.IsTrue(quote.Attributes.Count == 3);
			
			Assert.AreEqual("After landing the cloaked Klingon bird of prey in Golden Gate park:", quote.Attributes.Title.Text);

			AsciiDocAssert.AreEqual(text, document);
		}
	}
}