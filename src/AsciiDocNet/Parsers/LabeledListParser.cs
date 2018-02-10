using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class LabeledListParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.LabeledListItem.IsMatch(reader.Line.AsString());

        public override void InternalParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.LabeledListItem.Match(reader.Line.AsString());
            if (!match.Success)
            {
                throw new ArgumentException("not a labeled list item");
            }

            var label = match.Groups["label"].Value;
            var level = match.Groups["level"].Value.Length;

            // levels start at 0
            if (level > 0)
            {
                level -= 2;
            }

            var labeledListItem = new LabeledListItem(label, level);
            labeledListItem.Attributes.Add(attributes);

            var text = match.Groups["text"].Value;

            // labeled lists are lenient with whitespace so can have whitespace after the label
            // and before any content.
            if (!string.IsNullOrWhiteSpace(text))
            {
                buffer.Add(text);
                reader.ReadLine();
            }
            else
            {
                reader.ReadLine();
                while (reader.Line.AsString() != null &&
                       PatternMatcher.BlankCharacters.IsMatch(reader.Line.AsString()))
                {
                    reader.ReadLine();
                }
            }

            while (reader.Line.AsString() != null &&
                   //!PatternMatcher.ListItemContinuation.IsMatch(reader.Line.AsString()) &&
                   !PatternMatcher.BlankCharacters.IsMatch(reader.Line.AsString()) &&
                   !PatternMatcher.LabeledListItem.IsMatch(reader.Line.AsString()) &&
                   (delimiterRegex == null || !delimiterRegex.IsMatch(reader.Line.AsString())))
            {
                buffer.Add(reader.Line.AsString());
                reader.ReadLine();
            }

            // TODO: handle multi element list items (i.e. continued with +)
            // TODO: This may not be a paragraph.
            AttributeList a = null;
            ProcessParagraph(labeledListItem, ref buffer, ref a);

            LabeledList labeledList;
            if (container.Count > 0)
            {
                labeledList = container[container.Count - 1] as LabeledList;

                if (labeledList != null && labeledList.Items.Count > 0 && labeledList.Items[0].Level == labeledListItem.Level)
                {
                    labeledList.Items.Add(labeledListItem);
                }
                else
                {
                    labeledList = new LabeledList { Items = { labeledListItem } };
                    container.Add(labeledList);
                }
            }
            else
            {
                labeledList = new LabeledList { Items = { labeledListItem } };
                container.Add(labeledList);
            }

            attributes = null;
        }
    }
}