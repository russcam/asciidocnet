using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class ImageTests
	{
		private const string Image = "image::{imagesdir}/hadouken-indentation.jpg[dead indent]";

		[Theory]
		[InlineData(Image)]
		[InlineData(".title\n" + Image)]
		[InlineData("[name=\"value\"]\n" + Image)]
		[InlineData(".title\n[name=\"value\"]\n" + Image)]
		[InlineData("[name=\"value\"]\n.title\n" + Image)]
		public void ShouldParseImage(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Image>(document[0]);

			var image = (Image)document[0];

			Assert.Equal("{imagesdir}/hadouken-indentation.jpg", image.Path);
			Assert.Equal("dead indent", image.AlternateText);
		}

		[Fact]
		public void ShouldParseIndentedImageWithAnchorAndTitle()
		{
			var input = $@"[[indent]]
.hadouken indenting example
{Image}";

			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Image>(document[0]);

			var image = (Image)document[0];

			Assert.Equal("{imagesdir}/hadouken-indentation.jpg", image.Path);
			Assert.Equal("hadouken indenting example", image.Attributes.Title.Text);
		}		
		
		[Fact]
		public void ShouldParseImageWithAnchorAndTitle()
		{
			var input = "image::hadouken-indentation.jpg[hadouken indenting]";

			var document = Document.Parse(input);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);
			Assert.IsType<Image>(document[0]);

			var image = (Image)document[0];

			Assert.Equal("hadouken-indentation.jpg", image.Path);
			Assert.Equal("hadouken indenting", image.AlternateText);
			AsciiDocAssert.Equal(input, document);
		}
	}
}
