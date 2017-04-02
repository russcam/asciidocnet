using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CsQuery;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class DocumentTests : VisitorTestsBase
	{
	    private class HtmlDocument
	    {
            public string FolderOnDisk { get; set; }
            public string GithubListingUrl { get; set; }
            public string Branch { get; set; }
            public string GithubDownloadUrl(string file) => 
                this.GithubListingUrl.Replace("github.com", "raw.githubusercontent.com")
                                     .Replace("tree/", "") + "/" + file;
        }

	    private const string TopLevelDir = "docs";
	    private const string GithubRepository = "https://github.com/elastic/elasticsearch-net/tree/{commit}/" + TopLevelDir;
		private const string ShaCommit = "5.x";
		private static readonly bool FetchFiles = false;

        [Theory]
        [MemberData(nameof(Files))]
        public void ShouldParseDocument(FileInfo file)
        {
            var document = Document.Load(file.FullName);
            document.Accept(Visitor);

            var content = Builder.ToString();
            var visitedDocument = Document.Parse(content);
            Assert.Equal(document, visitedDocument);
        }

        public static IEnumerable<object[]> Files
		{
			get
			{
			    if (!Directory.Exists(TopLevelDir))
			        Directory.CreateDirectory(TopLevelDir);

			    var testFiles = Directory.EnumerateFiles(TopLevelDir, "*.asciidoc", SearchOption.AllDirectories);

				if (!testFiles.Any() || FetchFiles)
				{
				    var document = new HtmlDocument
				    {
				        Branch = ShaCommit,
				        FolderOnDisk = TopLevelDir,
				        GithubListingUrl = GithubRepository.Replace("{commit}", ShaCommit)
				    };

				    DownloadAsciiDocFiles(document);

                    testFiles = Directory.EnumerateFiles(TopLevelDir, "*.asciidoc", SearchOption.AllDirectories);
                }

				return testFiles.Select(testFile => new object[] { new FileInfo(testFile) });
			}
		}

		private static void DownloadAsciiDocFiles(HtmlDocument htmlDocument)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var html = client.DownloadString(htmlDocument.GithubListingUrl);
                    FindAsciiDocFiles(htmlDocument, html);
                }
                catch
                {
                }
            }
        }

        private static void FindAsciiDocFiles(HtmlDocument htmlDocument, string html)
        {
            if (!Directory.Exists(htmlDocument.FolderOnDisk))
                Directory.CreateDirectory(htmlDocument.FolderOnDisk);

            var dom = CQ.Create(html);

            var documents = dom[".js-navigation-open"]
                .Select(s => s.InnerText)
                .Where(s => !string.IsNullOrEmpty(s) && s.EndsWith(".asciidoc"))
                .ToList();

            documents.ForEach(s => WriteAsciiDoc(htmlDocument, s));

            var directories = dom[".js-navigation-open"]
                .Select(s => s.InnerText)
                .Where(s => !string.IsNullOrWhiteSpace(s) && s.IndexOf(".") == -1 && Path.GetExtension(s) == string.Empty)
                .ToList();

            directories.ForEach(directory =>
            {
                var subDirectory = new HtmlDocument
                {
                    Branch = htmlDocument.Branch,
                    FolderOnDisk = Path.Combine(htmlDocument.FolderOnDisk, directory),
                    GithubListingUrl = htmlDocument.GithubListingUrl + "/" + directory
                };

                DownloadAsciiDocFiles(subDirectory);
            });
        }

        private static void WriteAsciiDoc(HtmlDocument html, string s)
        {
            var rawFile = html.GithubDownloadUrl(s);
            using (var client = new WebClient())
            {
                var fileName = rawFile.Split('/').Last();
                var contents = client.DownloadString(rawFile);
                File.WriteAllText(Path.Combine(html.FolderOnDisk, fileName), contents);
            }
        }
	}
}