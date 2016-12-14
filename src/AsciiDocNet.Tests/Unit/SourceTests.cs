using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class SourceTests : DelimitedTextElementTests<Source>
	{
		[Fact]
		public void ShouldParseDelimitedWithCallouts()
		{
			var text = $@"[{Name}]
{Delimiter}
{Text}
{Delimiter}
<1> Callout 1
<2> Callout 2
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<Source>(document[0]);
			var element = (Source)document[0];

			Assert.Equal(Text, element.Text);

			Assert.True(element.Callouts.Count == 2);

			Assert.Equal(1, element.Callouts[0].Number);
			Assert.Equal("Callout 1", element.Callouts[0].Text);
			Assert.Equal(2, element.Callouts[1].Number);
			Assert.Equal("Callout 2", element.Callouts[1].Text);
		}
	}
}