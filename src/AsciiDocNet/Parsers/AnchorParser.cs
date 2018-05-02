using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class AnchorParser : IMatchingElementParser
    {
        public bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) => 
            PatternMatcher.Anchor.IsMatch(reader.Line);

        public void Parse(
            Container container,
            IDocumentReader reader,
            Func<string, bool> predicate,
            ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.Anchor.Match(reader.Line);
            if (!match.Success)
            {
                throw new ArgumentException("not an anchor");
            }

            var id = match.Groups["id"].Value;

            var reference = !string.IsNullOrEmpty(match.Groups["reference"].Value) 
                ? match.Groups["reference"].Value 
                : null;

            var anchor = new Anchor(id, reference);

            if (attributes != null)
            {
                attributes.Add(anchor);
            }
            else
            {
                attributes = new AttributeList { anchor };
            }

            reader.ReadLine();
        }
    }
}