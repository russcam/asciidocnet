using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class QuoteParagraphTests
	{
		[Fact]
		public void ShouldParse()
		{
			var text = @".After landing the cloaked Klingon bird of prey in Golden Gate park:
[quote, Captain James T. Kirk, Star Trek IV: The Voyage Home]     
Everybody remember where we parked.";

			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<Quote>(document[0]);
			var quote = (Quote)document[0];

			Assert.True(quote.Count == 1);
			Assert.IsType<Paragraph>(quote[0]);		
			Assert.True(quote.Attributes.Count == 3);
			Assert.Equal("After landing the cloaked Klingon bird of prey in Golden Gate park:", quote.Attributes.Title.Text);
		}

		[Fact]
		public void ShouldParseWithFloat()
		{
			var text = @"[[captain-quote]]
[quote, Captain James T. Kirk, Star Trek IV: The Voyage Home]
.After landing the cloaked Klingon bird of prey in Golden Gate park:
Everybody remember where we parked.";

			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<Quote>(document[0]);
			var quote = (Quote)document[0];

			Assert.True(quote.Count == 1);
			Assert.IsType<Paragraph>(quote[0]);
			Assert.True(quote.Attributes.Count == 3);
			
			Assert.Equal("After landing the cloaked Klingon bird of prey in Golden Gate park:", quote.Attributes.Title.Text);

			AsciiDocAssert.Equal(text, document);
		}
	}
}