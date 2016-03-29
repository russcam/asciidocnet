using System;
using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public abstract class DelimitedPairContainerInlineElementTests<TElement> where TElement : IContainerInlineElement
	{
		public virtual string Text { get; } = "This is a paragraph";

		public abstract Tuple<string, string>[] DelimiterPairs { get; }

		[Test]
		[TestCaseSource(nameof(DelimiterPairs))]
		public void ShouldParseInlineElement(Tuple<string, string> delimiterPair)
		{
			var text = $"{delimiterPair.Item1}{Text}{delimiterPair.Item2}";
			var document = Document.Parse(text);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(1, paragraph.Elements.Count);
			Assert.IsInstanceOf<TElement>(paragraph.Elements[0]);

			var element = (TElement)paragraph.Elements[0];
			Assert.AreEqual(Text, ((TextLiteral)element.Elements[0]).Text);
		}
	}


	[TestFixture]
	public abstract class DelimitedPairInlineElementTests<TElement> where TElement : IInlineElement, IText
	{
		public virtual string Text { get; } = "This is a paragraph";

		public abstract Tuple<string, string>[] DelimiterPairs { get; }

		[Test]
		[TestCaseSource(nameof(DelimiterPairs))]
		public void ShouldParseInlineElement(Tuple<string, string> delimiterPair)
		{
			var text = $"{delimiterPair.Item1}{Text}{delimiterPair.Item2}";
			var document = Document.Parse(text);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(1, paragraph.Elements.Count);
			Assert.IsInstanceOf<TElement>(paragraph.Elements[0]);

			var element = (TElement)paragraph.Elements[0];
			Assert.AreEqual(Text, element.Text);
		}
	}
}