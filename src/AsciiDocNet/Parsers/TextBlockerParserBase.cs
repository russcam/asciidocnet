using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class TextBlockerParserBase<TElement> : ProcessBufferParserBase 
        where TElement : IText, IElement, IAttributable, new()
    {
        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var elementDelimiter = PatternMatcher.GetDelimiterRegexFor<TElement>();
            var isDelimiter = elementDelimiter.IsMatch(reader.Line);

            if (isDelimiter)
            {
                AttributeList a = null;
                ProcessParagraph(container, ref buffer, ref a);
                reader.ReadLine();
                while (reader.Line != null && !elementDelimiter.IsMatch(reader.Line))
                {
                    buffer.Add(reader.Line);
                    reader.ReadLine();
                }
            }
            else
            {
                buffer.Add(reader.Line);
                reader.ReadLine();
                while (reader.Line != null && !PatternMatcher.BlankCharacters.IsMatch(reader.Line))
                {
                    buffer.Add(reader.Line);
                    reader.ReadLine();
                }
            }

            var element = new TElement { Text = string.Join(Environment.NewLine, buffer) };
            element.Attributes.Add(attributes);
            container.Add(element);
            attributes = null;
            buffer = new List<string>(8);

            reader.ReadLine();
        }
    }
}