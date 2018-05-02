using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class BlockParserBase<TElement> : DescendingParserBase, IMatchingElementParser
        where TElement : Container, IElement, IAttributable, new()
    {
        public abstract bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes);

        public override void Parse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var delimiterRegex = PatternMatcher.GetDelimiterRegexFor<TElement>();
            var element = new TElement();
            element.Attributes.Add(attributes);
            if (delimiterRegex.IsMatch(reader.Line))
            {
                ProcessParagraph(container, ref buffer);
                reader.ReadLine();
                Parse(element, reader, delimiterRegex);
            }
            else
            {
                ProcessParagraph(element, ref buffer);
                Parse(element, reader, PatternMatcher.BlankCharacters);
            }

            container.Add(element);
            attributes = null;

            reader.ReadLine();
        }

        private void Parse(Container parent, IDocumentReader reader, Regex delimiterRegex)
        {
            var buffer = new List<string>(8);
            AttributeList attributes = null;
            DescendingParse(parent, reader, delimiterRegex.IsMatch, ref buffer, ref attributes);
        }
    }
}