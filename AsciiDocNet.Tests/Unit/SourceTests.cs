using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class SourceTests : DelimitedTextElementTests<Source>
	{
		[Test]
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
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<Source>(document[0]);
			var element = (Source)document[0];

			Assert.AreEqual(Text, element.Text);

			Assert.IsTrue(element.Callouts.Count == 2);

			Assert.AreEqual(1, element.Callouts[0].Number);
			Assert.AreEqual("Callout 1", element.Callouts[0].Text);
			Assert.AreEqual(2, element.Callouts[1].Number);
			Assert.AreEqual("Callout 2", element.Callouts[1].Text);
		}
	}
}