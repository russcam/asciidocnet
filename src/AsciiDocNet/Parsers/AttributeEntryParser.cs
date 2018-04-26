using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class AttributeEntryParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.AttributeEntry.IsMatch(reader.Line);

        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var attributeEntry = ParseAttributeEntry(reader.Line);
            var document = container as Document;

            if (document != null)
            {
                if (document.Count == 0)
                    document.Attributes.Add(attributeEntry);
                else
                    document.Add(attributeEntry);
            }
            else
                container.Add(attributeEntry);

            reader.ReadLine();
        }

        private static AttributeEntry ParseAttributeEntry(string input)
        {
            var match = PatternMatcher.AttributeEntry.Match(input);
            if (!match.Success)
            {
                throw new ArgumentException("not an attribute entry");
            }

            var name = match.Groups["name"].Value.ToLowerInvariant();
            AttributeEntry attributeEntry;
            if (name.StartsWith("!"))
            {
                attributeEntry = new UnsetAttributeEntry(name.Substring(1));
            }
            else if (name.EndsWith("!"))
            {
                attributeEntry = new UnsetAttributeEntry(name.Substring(0, name.Length - 1));
            }
            else
            {
                switch (name)
                {
                    case "author":
                        attributeEntry = new AuthorInfoAttributeEntry(match.Groups["value"].Value);
                        break;
                    default:
                        attributeEntry = new AttributeEntry(name, match.Groups["value"].Value);
                        break;
                }
            }

            return attributeEntry;
        }
    }
}