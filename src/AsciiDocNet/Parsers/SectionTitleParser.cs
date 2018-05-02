using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class SectionTitleParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attrtibutes) =>
            PatternMatcher.SectionTitle.IsMatch(reader.Line);

        // TODO: based on html output, a section title should define a section block element into which all proceeding elements should be added, until the next section Title is hit
        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.SectionTitle.Match(reader.Line);
            if (!match.Success)
            {
                throw new ArgumentException("not a section title");
            }

            var title = match.Groups["title"].Value;
            var inlineElements = ProcessInlineElements(title);
            var level = match.Groups["level"].Value.Length;
            var sectionTitle = new SectionTitle(inlineElements, level);
            sectionTitle.Attributes.Add(attributes);
            container.Add(sectionTitle);
            attributes = null;

            reader.ReadLine();
        }
    }
}