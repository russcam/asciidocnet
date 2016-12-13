using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class ImageTests
	{
		private const string Image = "image::{imagesdir}/hadouken-indentation.jpg[dead indent]";

		[Test]
		[TestCase(Image)]
		[TestCase(".title\n" + Image)]
		[TestCase("[name=\"value\"]\n" + Image)]
		[TestCase(".title\n[name=\"value\"]\n" + Image)]
		[TestCase("[name=\"value\"]\n.title\n" + Image)]
		public void ShouldParseImage(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Image>(document[0]);

			var image = (Image)document[0];

			Assert.AreEqual("{imagesdir}/hadouken-indentation.jpg", image.Path);
			Assert.AreEqual("dead indent", image.AlternateText);
		}

		[Test]
		public void ShouldParseImageWithAnchorAndTitle()
		{
			var input = $@"[[indent]]
.hadouken indenting example
{Image}";

			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<Image>(document[0]);

			var image = (Image)document[0];

			Assert.AreEqual("{imagesdir}/hadouken-indentation.jpg", image.Path);
			Assert.AreEqual("hadouken indenting example", image.Attributes.Title.Text);
		}
	}
}