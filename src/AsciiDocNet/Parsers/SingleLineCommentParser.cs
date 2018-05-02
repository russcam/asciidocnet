using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class SingleLineCommentParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.CommentLine.IsMatch(reader.Line);

        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var comment = new Comment(reader.Line.Substring(2));
            container.Add(comment);

            reader.ReadLine();
        }
    }
}