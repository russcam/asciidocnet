using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class TextBlockerParserBase<TElement> : ProcessBufferParserBase 
        where TElement : IText, IElement, IAttributable, new()
    {
	    protected override void InternalParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var elementDelimiter = PatternMatcher.GetDelimiterRegexFor<TElement>();
            var isDelimiter = elementDelimiter.IsMatch(reader.Line.AsString());

            if (isDelimiter)
            {
                AttributeList a = null;
                ProcessParagraph(container, ref buffer, ref a);
                reader.ReadLine();
                while (reader.Line.AsString() != null && !elementDelimiter.IsMatch(reader.Line.AsString()))
                {
                    buffer.Add(reader.Line.AsString());
                    reader.ReadLine();
                }
            }
            else
            {
                buffer.Add(reader.Line.AsString());
                reader.ReadLine();
                while (reader.Line.AsString() != null && !PatternMatcher.BlankCharacters.IsMatch(reader.Line.AsString()))
                {
                    buffer.Add(reader.Line.AsString());
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