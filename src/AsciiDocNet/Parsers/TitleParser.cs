using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class TitleParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Title.IsMatch(reader.Line);

        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.Title.Match(reader.Line);
            if (!match.Success)
                throw new ArgumentException("not a block title");

            var title = new Title(match.Groups["title"].Value);
            if (attributes != null)
                attributes.Add(title);
            else
                attributes = new AttributeList {title};

            reader.ReadLine();
        }
    }
}