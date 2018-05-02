using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class ListingParserBase<TElement> : ProcessBufferParserBase
        where TElement : Listing, IElement, new()
    {
        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var listingRegex = PatternMatcher.GetDelimiterRegexFor<TElement>();
            var isDelimiter = listingRegex.IsMatch(reader.Line);

            if (isDelimiter)
            {
                ProcessParagraph(container, ref buffer);
                reader.ReadLine();
                while (reader.Line != null && !listingRegex.IsMatch(reader.Line))
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

            reader.ReadLine();
            if (isDelimiter)
            {
                while (reader.Line != null && PatternMatcher.Callout.IsMatch(reader.Line))
                {
                    ParseCallout(element, reader.Line);
                    reader.ReadLine();
                }
            }

            container.Add(element);
            attributes = null;
            buffer = new List<string>(8);
        }

        private void ParseCallout(Listing element, string line)
        {
            var match = PatternMatcher.Callout.Match(line);
            if (!match.Success)
            {
                throw new ArgumentException("not a callout");
            }

            var number = int.Parse(match.Groups["number"].Value);
            var text = match.Groups["text"].Value;
            element.Callouts.Add(new Callout(number, text));
        }
    }
}