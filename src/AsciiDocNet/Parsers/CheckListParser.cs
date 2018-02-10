using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class CheckListParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.CheckListItem.IsMatch(reader.Line.AsString());

        public override void InternalParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.CheckListItem.Match(reader.Line.AsString());
            if (!match.Success)
            {
                throw new ArgumentException("not a check list item");
            }

            var level = match.Groups["level"].Value;
            var isChecked = !string.IsNullOrWhiteSpace(match.Groups["checked"].Value);
            var text = match.Groups["text"].Value;

            var listItem = new CheckListItem(level.Length, isChecked);
            listItem.Attributes.Add(attributes);

            buffer.Add(text);
            reader.ReadLine();

            while (reader.Line.AsString() != null &&
                   !PatternMatcher.ListItemContinuation.IsMatch(reader.Line.AsString()) &&
                   !PatternMatcher.BlankCharacters.IsMatch(reader.Line.AsString()) &&
                   !PatternMatcher.CheckListItem.IsMatch(reader.Line.AsString()) &&
                   !PatternMatcher.ListItem.IsMatch(reader.Line.AsString()) &&
                   (delimiterRegex == null || !delimiterRegex.IsMatch(reader.Line.AsString())))
            {
                buffer.Add(reader.Line.AsString());
                reader.ReadLine();
            }

            // TODO: handle list item continuations (i.e. continued with +)
            AttributeList a = null;
            ProcessParagraph(listItem, ref buffer, ref a);

            UnorderedList unorderedList;
            if (container.Count > 0)
            {
                unorderedList = container[container.Count - 1] as UnorderedList;

                if (unorderedList != null && unorderedList.Items.Count > 0 && unorderedList.Items[0].Level == listItem.Level)
                {
                    unorderedList.Items.Add(listItem);
                }
                else
                {
                    unorderedList = new UnorderedList { Items = { listItem } };
                    container.Add(unorderedList);
                }
            }
            else
            {
                unorderedList = new UnorderedList { Items = { listItem } };
                container.Add(unorderedList);
            }

            attributes = null;
        }
    }
}