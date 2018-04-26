using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class DocumentTitleParser : IMatchingElementParser
    {
        public bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.DocumentTitle.IsMatch(reader.Line) && 
            (reader.LineNumber == 1 || container.GetType() == typeof(Document) && container.All(e => e is Comment));

        public void Parse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var document = (Document)container;
            ParseDocumentTitle(document, reader, ref attributes);

            reader.ReadLine();

            if (reader.Line == null)
                return;

            ParseAuthors(document, reader.Line);

            reader.ReadLine();
        }

        private static void ParseDocumentTitle(Document document, IDocumentReader reader, ref AttributeList attributes)
        {
            var match = PatternMatcher.DocumentTitle.Match(reader.Line);
            if (!match.Success)
            {
                throw new ArgumentException("not a document title");
            }

            var title = match.Groups["title"].Value;
            var lastColonIndex = title.LastIndexOf(":", StringComparison.OrdinalIgnoreCase);
            var documentTitle = lastColonIndex > -1
                ? new DocumentTitle(title.Substring(0, lastColonIndex), title.Substring(lastColonIndex + 1))
                : new DocumentTitle(title);

            documentTitle.Attributes.Add(attributes);
            document.Title = documentTitle;

            attributes = null;
        }

        private static void ParseAuthors(Document document, string line)
        {
            if (line.IndexOf(';') > -1)
            {
                var authors = line.Split(';');
                foreach (var author in authors)
                {
                    if (PatternMatcher.AuthorInfo.IsMatch(author))
                    {
                        document.Authors.Add(ParseAuthor(author));
                    }
                }
            }
            else if (PatternMatcher.AuthorInfo.IsMatch(line))
            {
                document.Authors.Add(ParseAuthor(line));
            }
        }

        private static AuthorInfo ParseAuthor(string input)
        {
            var match = PatternMatcher.AuthorInfo.Match(input);
            if (!match.Success)
            {
                throw new ArgumentException("not an author info");
            }

            var author = new AuthorInfo
            {
                FirstName = match.Groups["firstname"].Value.Replace("_", " "),
                Email = string.IsNullOrEmpty(match.Groups["email"].Value) ? null : match.Groups["email"].Value
            };

            var middle = string.IsNullOrEmpty(match.Groups["middlename"].Value) 
                ? null 
                : match.Groups["middlename"].Value;

            var last = string.IsNullOrEmpty(match.Groups["lastname"].Value) 
                ? null 
                : match.Groups["lastname"].Value;

            if (middle != null && last != null)
            {
                author.MiddleName = middle.Replace("_", " ");
                author.LastName = last.Replace("_", " ");
            }
            else if (middle != null)
            {
                author.LastName = middle.Replace("_", " ");
            }

            return author;
        }
    }
}