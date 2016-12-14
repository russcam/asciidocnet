using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class DocumentTitleTests
	{
		private const string Title = "Document Title";
		private const string Subtitle = "Sub title";

		private const string DocumentTitle = "= " + Title;
		private const string DocumentWithSubtitle = DocumentTitle + ":" + Subtitle;

		public static IEnumerable<object[]> Titles
		{
			get
			{
				yield return new object[] { DocumentTitle };

				var allowedPrecedingEntries = new List<string>
				{
					"\n",
					"// This is a single comment",
					":exampleattribute: example attribute"
				};

				foreach (var entry in allowedPrecedingEntries)
				{
					yield return new object[] { $"{entry}\n{DocumentTitle}" };
				}

				foreach (var entry in allowedPrecedingEntries.Zip(allowedPrecedingEntries, (first, second) => first + "\n" + second))
				{
					yield return new object[] { $"{entry}\n{DocumentTitle}" };
				}
			}
		}

		public static IEnumerable<object[]> TitlesWithSubtitle
		{
			get
			{
				yield return new object[] { DocumentWithSubtitle };

				var allowedPrecedingEntries = new List<string>
				{
					"\n",
					"// This is a single comment",
					":exampleattribute: example attribute"
				};

				foreach (var entry in allowedPrecedingEntries)
				{
					yield return new object[] { $"{entry}\n{DocumentWithSubtitle}" };
				}

				foreach (var entry in allowedPrecedingEntries.Zip(allowedPrecedingEntries, (first, second) => first + "\n" + second))
				{
					yield return new object[] { $"{entry}\n{DocumentWithSubtitle}" };
				}
			}
		}

		[Theory]
		[MemberData(nameof(Titles))]
		public void ShouldParseDocumentTitle(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document.Title);
			Assert.Equal(Title, document.Title.Title);
		}

		[Theory]
		[MemberData(nameof(TitlesWithSubtitle))]
		public void ShouldParseDocumentTitleWithSubtitle(string input)
		{
			var document = Document.Parse(input);

			Assert.NotNull(document.Title);
			Assert.Equal(Title, document.Title.Title);
			Assert.Equal(Subtitle, document.Title.Subtitle);
		}
	}
}
