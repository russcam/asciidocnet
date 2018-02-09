using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                                     .Replace("tree/", string.Empty) + "/" + file;
        }

		private static readonly string RelativePathDocs = Path.GetFullPath("../../../../../" + TopLevelDir);
	    private const string TopLevelDir = "docs";
	    private const string GithubRepository = "https://github.com/elastic/elasticsearch-net/tree/{commit}/" + TopLevelDir;
		private const string ShaCommit = "6.x";
		private static readonly bool FetchFiles = false;

        [Theory]
        [MemberData(nameof(Files))]
        public void ShouldParseDocument(FileInfo file)
        {
            var document = Document.Load(file.FullName);
            document.Accept(Visitor);

            var content = Builder.ToString();
            var visitedDocument = Document.Parse(content);

	        // write out visited doc for diff
	        var visitedFile = file.FullName.Replace(@"\docs\", @"\visited-docs\");
	        var visitedDirectory = Path.GetDirectoryName(visitedFile);
	        if (!Directory.Exists(visitedDirectory))
	        {
		        Directory.CreateDirectory(visitedDirectory);
	        }        
	        File.WriteAllText(visitedFile, content);   
	        
	        Assert.Equal(document, visitedDocument);
        }

        public static IEnumerable<object[]> Files
		{
			get
			{
			    if (!Directory.Exists(RelativePathDocs))
			        Directory.CreateDirectory(RelativePathDocs);

			    var testFiles = Directory.EnumerateFiles(RelativePathDocs, "*.asciidoc", SearchOption.AllDirectories);

				if (!testFiles.Any() || FetchFiles)
				{
					var githubListingUrl = GithubRepository.Replace("{commit}", ShaCommit);				
					Console.WriteLine($"Fetching documents from {githubListingUrl} to parse");
					var document = new HtmlDocument
				    {
				        Branch = ShaCommit,
				        FolderOnDisk = RelativePathDocs,
				        GithubListingUrl = githubListingUrl
				    };

				    DownloadAsciiDocFiles(document);

                    testFiles = Directory.EnumerateFiles(RelativePathDocs, "*.asciidoc", SearchOption.AllDirectories);
                }

				return testFiles.Select(testFile => new object[] { new FileInfo(testFile) });
			}
		}

		private static void DownloadAsciiDocFiles(HtmlDocument htmlDocument)
        {
            using (var client = new HttpClient())
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

            var documents = dom["td.content .js-navigation-open"]
                .Select(s => s.InnerText)
                .Where(s => !string.IsNullOrEmpty(s) && s.EndsWith(".asciidoc"))
                .ToList();

            documents.ForEach(s => WriteAsciiDoc(htmlDocument, s));

            var directories = dom["td.content .js-navigation-open"]
                .Select(s => s.InnerText)
                .Where(s => !string.IsNullOrWhiteSpace(s) && !Path.HasExtension(s))
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
            using (var client = new HttpClient())
            {
                var fileName = rawFile.Split('/').Last();
                var contents = client.DownloadString(rawFile);
                File.WriteAllText(Path.Combine(html.FolderOnDisk, fileName), contents);
            }
        }
	}
}