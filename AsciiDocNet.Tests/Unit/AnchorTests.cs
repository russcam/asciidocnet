using System;
using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public abstract class AnchorTests<TElement> where TElement : Anchor, IInlineElement
	{
		private const string Id = "anchor-id";
		private const string XRefLabel = "reference";

		public abstract Tuple<string,string> DelimiterPair { get; }

		[Test]
		public void ShouldParseWithId()
		{
			var document = Document.Parse(DelimiterPair.Item1 + Id + DelimiterPair.Item2 + "reference");

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(2, paragraph.Elements.Count);
			Assert.IsInstanceOf<TElement>(paragraph.Elements[0]);

			var element = (TElement)paragraph.Elements[0];

			Assert.AreEqual(Id, element.Id);
		}

		[Test]
		public void ShouldParseWithIdAndReference()
		{
			var document = Document.Parse(DelimiterPair.Item1 + Id + "," +XRefLabel + DelimiterPair.Item2 + "reference");

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(2, paragraph.Elements.Count);
			Assert.IsInstanceOf<TElement>(paragraph.Elements[0]);

			var element = (TElement)paragraph.Elements[0];

			Assert.AreEqual(Id, element.Id);
			Assert.AreEqual(XRefLabel, element.XRefLabel);
		}

		[Test]
		public void ShouldParseInsideParagraphWithIdAndReference()
		{
			var document = Document.Parse("This is a " + DelimiterPair.Item1 + Id + "," + XRefLabel + DelimiterPair.Item2 + "reference");

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(3, paragraph.Elements.Count);
			Assert.IsInstanceOf<TElement>(paragraph.Elements[1]);

			var element = (TElement)paragraph.Elements[1];

			Assert.AreEqual(Id, element.Id);
			Assert.AreEqual(XRefLabel, element.XRefLabel);
		}
	}
}