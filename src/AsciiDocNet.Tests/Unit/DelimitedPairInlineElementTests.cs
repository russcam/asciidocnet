using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class DelimitedPairContainerInlineElementTests<TElement, TPairs> : ClassDataBase<TPairs>
		where TElement : InlineContainer
		where TPairs : ReusableClassData, new()
	{
		public virtual string Text { get; } = "This is a paragraph";

		[Theory]
		[MemberData(nameof(ClassData))]
		public void ShouldParseInlineElement(Tuple<string, string> delimiterPair)
		{
			var text = $"{delimiterPair.Item1}{Text}{delimiterPair.Item2}";
			var document = Document.Parse(text);

			Assert.True(1 == document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.True(1 == paragraph.Count);
			Assert.IsType<TElement>(paragraph[0]);

			var element = (TElement)paragraph[0];
			Assert.Equal(Text, ((TextLiteral)element[0]).Text);
		}
	}


	public abstract class DelimitedPairInlineElementTests<TElement, TPairs> : ClassDataBase<TPairs>
		where TElement : IInlineElement, IText
		where TPairs : ReusableClassData, new()
	{
		public virtual string Text { get; } = "This is a paragraph";

		[Theory]
		[MemberData(nameof(ClassData))]
		public void ShouldParseInlineElement(Tuple<string, string> delimiterPair)
		{
			var text = $"{delimiterPair.Item1}{Text}{delimiterPair.Item2}";
			var document = Document.Parse(text);

			Assert.True(1 == document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.True(1 == paragraph.Count);
			Assert.IsType<TElement>(paragraph[0]);

			var element = (TElement)paragraph[0];
			Assert.Equal(Text, element.Text);
		}
	}
}