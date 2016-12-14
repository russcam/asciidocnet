using System;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class AnchorTests<TElement> where TElement : Anchor, IInlineElement
	{
		private const string Id = "anchor-id";
		private const string XRefLabel = "reference";

		public abstract Tuple<string,string> DelimiterPair { get; }

		[Fact]
		public void ShouldParseWithId()
		{
			var document = Document.Parse(DelimiterPair.Item1 + Id + DelimiterPair.Item2 + "reference");

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.Equal(2, paragraph.Count);
			Assert.IsType<TElement>(paragraph[0]);

			var element = (TElement)paragraph[0];

			Assert.Equal(Id, element.Id);
		}

		[Fact]
		public void ShouldParseWithIdAndReference()
		{
			var document = Document.Parse(DelimiterPair.Item1 + Id + "," +XRefLabel + DelimiterPair.Item2 + "reference");

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.Equal(2, paragraph.Count);
			Assert.IsType<TElement>(paragraph[0]);

			var element = (TElement)paragraph[0];

			Assert.Equal(Id, element.Id);
			Assert.Equal(XRefLabel, element.XRefLabel);
		}

		[Fact]
		public void ShouldParseInsideParagraphWithIdAndReference()
		{
			var document = Document.Parse("This is a " + DelimiterPair.Item1 + Id + "," + XRefLabel + DelimiterPair.Item2 + "reference");

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.Equal(3, paragraph.Count);
			Assert.IsType<TElement>(paragraph[1]);

			var element = (TElement)paragraph[1];

			Assert.Equal(Id, element.Id);
			Assert.Equal(XRefLabel, element.XRefLabel);
		}
	}
}