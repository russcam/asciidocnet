using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Octokit;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class DocumentTests : VisitorTestsBase
	{
		private const string RepositoryName = "elasticsearch-net";
		private const string RepositoryOwner = "elastic";
		private const string ShaCommit = "98d4fb99ed9e20258b6180358fbc5988b0c3c43b";

		private const string TestDocumentsDir = "test-documents";
		private readonly bool FetchFiles = true;

		public IEnumerable<FileInfo> Files
		{
			get
			{
				if (!Directory.Exists(TestDocumentsDir))
				{
					Directory.CreateDirectory(TestDocumentsDir);
				}

				var testFiles = Directory.EnumerateFiles(TestDocumentsDir, "*.asciidoc", SearchOption.AllDirectories);

				if (FetchFiles)
				{
					var baseAddress = new Uri("https://api.github.com");
					var credentialsInBin = File.Exists("credentials.json");
					var credentialsInRoot = File.Exists(@"..\..\credentials.json");

					if (credentialsInBin || credentialsInRoot)
					{
						var credentials = JsonConvert.DeserializeObject<JObject>(
							File.ReadAllText(credentialsInBin ? "credentials.json": @"..\..\credentials.json"));

						var clientId = credentials["Github:ClientId"].Value<string>();
						var clientSecret = credentials["Github:ClientSecret"].Value<string>();

						if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
						{
							var builder = new UriBuilder("https://api.github.com") { Port = -1 };
							var query = HttpUtility.ParseQueryString(builder.Query);
							query["client_id"] = clientId;
							query["client_secret"] = clientSecret;
							builder.Query = query.ToString();
							baseAddress = builder.Uri;
						}
					}

					var github = new GitHubClient(new Connection(new ProductHeaderValue("asciidocnet"), baseAddress));
					TreeResponse tree;

					try
					{
						tree = github.Git.Tree
							.GetRecursive(RepositoryOwner, RepositoryName, ShaCommit)
							.Result;
					}
					catch (AggregateException ae)
					{
						if (ae.InnerException is RateLimitExceededException)
						{
							return testFiles.Select(testFile => new FileInfo(testFile));
						}

						throw;
					}
		
					var asciidocs = tree.Tree.Where(i =>
						i.Path.StartsWith("docs/asciidoc") && i.Path.EndsWith(".asciidoc"));

					foreach (var asciidoc in asciidocs)
					{
						var directory = Path.Combine(TestDocumentsDir, Path.GetDirectoryName(asciidoc.Path));

						if (!Directory.Exists(directory))
						{
							Directory.CreateDirectory(directory);
						}

						var path = Path.Combine(directory, Path.GetFileName(asciidoc.Path));

						if (!File.Exists(path))
						{
							try
							{
								var blob = github.Git.Blob.Get(RepositoryOwner, RepositoryName, asciidoc.Sha).Result;
								var content = GetContent(blob.Content);
								File.WriteAllText(path, content);
							}
							catch (AggregateException ae)
							{
								if (ae.InnerException is RateLimitExceededException)
								{
									break;
								}

								throw;
							}
						}
					}
				}

				return testFiles.Select(testFile => new FileInfo(testFile));
			}
		}

		[Test]
		[TestCaseSource(nameof(Files))]
		public void ShouldParseDocument(FileInfo file)
		{
			var document = Document.Load(file.FullName);
			document.Accept(Visitor);
			File.WriteAllText(file.Name, Builder.ToString());
		}

		private static string GetContent(string base64Encoded)
		{
			var bytes = Convert.FromBase64String(base64Encoded);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}