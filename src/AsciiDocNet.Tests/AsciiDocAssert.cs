using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using AsciiDocNet.Tests.Extensions;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace AsciiDocNet.Tests
{
	public class AsciiDocAssert
	{
		public static void Equal(string asciidoc, Document document)
		{
			var expected = asciidoc.ConsistentLineEndings().RemoveTrailingNewLine();
			var actual = RenderAsciiDoc(document);

			Diff(expected, actual);
			Assert.True(true);
		}
		
		public static void Equal(Document first, Document second)
		{
			var firstAsciidoc = RenderAsciiDoc(first);
			var secondAsciidoc = RenderAsciiDoc(second);
			Diff(firstAsciidoc, secondAsciidoc);
			Assert.True(true);
		}

		private static string RenderAsciiDoc(Document document)
		{
			var directoryAttributes = document.Attributes.Where(a => a.Name == "docdir").ToList();
			foreach (var directoryAttribute in directoryAttributes)
			{
				document.Attributes.Remove(directoryAttribute);
			}

			var builder = new StringBuilder();
			using (var visitor = new AsciiDocVisitor(new StringWriter(builder)))
			{
				document.Accept(visitor);
			}

			return builder.ToString().RemoveTrailingNewLine();
		}

		private static void Diff(string expected, string actual, string message = null)
		{
			var d = new Differ();
			var inlineBuilder = new InlineDiffBuilder(d);
			var result = inlineBuilder.BuildDiffModel(expected, actual);
			var hasChanges = result.Lines.Any(l => l.Type != ChangeType.Unchanged);
			if (!hasChanges) return;

			var diff = result.Lines.Aggregate(new StringBuilder().AppendLine(message), (sb, line) =>
			{
				if (line.Type == ChangeType.Inserted)
					sb.Append("+ ");
				else if (line.Type == ChangeType.Deleted)
					sb.Append("- ");
				else
					sb.Append("  ");
				sb.AppendLine(line.Text);
				return sb;
			}, sb => sb.ToString());

			throw new Exception(diff);
		}
	}
}
