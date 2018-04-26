using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class AdmonitionParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Admonition.IsMatch(reader.Line);

        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.Admonition.Match(reader.Line);

            if (!match.Success)
            {
                throw new ArgumentException("not an admonition");
            }

            buffer.Add(match.Groups["text"].Value);
            reader.ReadLine();
            while (reader.Line != null && !PatternMatcher.BlankCharacters.IsMatch(reader.Line))
            {
                buffer.Add(reader.Line);
                reader.ReadLine();
            }

            var admonition = new Admonition(match.Groups["style"].Value.ToEnum<AdmonitionStyle>());
            admonition.Attributes.Add(attributes);
            ProcessParagraph(admonition, ref buffer);
            container.Add(admonition);
            attributes = null;

            reader.ReadLine();
        }
    }
}