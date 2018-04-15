using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class OrderedListParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.OrderedListItem.IsMatch(reader.Line.AsString());

	    protected override void InternalParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.OrderedListItem.Match(reader.Line.AsString());
            if (!match.Success)
            {
                throw new ArgumentException("not an ordered list item");
            }

            var level = match.Groups["level"].Value;
            var orderedListItem = new OrderedListItem(level.Length);
            orderedListItem.Attributes.Add(attributes);

            var number = match.Groups["number"].Value;
            var upperAlpha = match.Groups["upperalpha"].Value;
            var lowerAlpha = match.Groups["loweralpha"].Value;
            var upperRoman = match.Groups["upperRoman"].Value;
            var lowerRoman = match.Groups["lowerRoman"].Value;

            if (!string.IsNullOrEmpty(number))
            {
                orderedListItem.Number = int.Parse(number);
            }
            else if (!string.IsNullOrEmpty(upperAlpha))
            {
                orderedListItem.Numbering = NumberStyle.UpperAlpha;
                orderedListItem.Number = Array.IndexOf(Patterns.UpperAlphabet, upperAlpha) + 1;
            }
            else if (!string.IsNullOrEmpty(lowerAlpha))
            {
                orderedListItem.Numbering = NumberStyle.LowerAlpha;
                orderedListItem.Number = Array.IndexOf(Patterns.LowerAlphabet, lowerAlpha) + 1;
            }
            else if (!string.IsNullOrEmpty(upperRoman))
            {
                orderedListItem.Numbering = NumberStyle.UpperRoman;
                orderedListItem.Number = RomanNumerals.ToInt(upperRoman);
            }
            else if (!string.IsNullOrEmpty(lowerRoman))
            {
                orderedListItem.Numbering = NumberStyle.LowerRoman;
                orderedListItem.Number = RomanNumerals.ToInt(lowerRoman);
            }

            var text = match.Groups["text"].Value;
            buffer.Add(text);
            reader.ReadLine();

            while (reader.Line.AsString() != null &&
                   !PatternMatcher.ListItemContinuation.IsMatch(reader.Line.AsString()) &&
                   !PatternMatcher.BlankCharacters.IsMatch(reader.Line.AsString()) &&
                   !PatternMatcher.OrderedListItem.IsMatch(reader.Line.AsString()) &&
                   (delimiterRegex == null || !delimiterRegex.IsMatch(reader.Line.AsString())))
            {
                buffer.Add(reader.Line.AsString());
                reader.ReadLine();
            }

            // TODO: handle multi element list items (i.e. continued with +)
            AttributeList a = null;
            ProcessParagraph(orderedListItem, ref buffer, ref a);

            OrderedList orderedList;
            if (container.Count > 0)
            {
                orderedList = container[container.Count - 1] as OrderedList;

                if (orderedList != null && orderedList.Items.Count > 0 && orderedList.Items[0].Level == orderedListItem.Level)
                {
                    orderedList.Items.Add(orderedListItem);
                }
                else
                {
                    orderedList = new OrderedList { Items = { orderedListItem } };
                    container.Add(orderedList);
                }
            }
            else
            {
                orderedList = new OrderedList { Items = { orderedListItem } };
                container.Add(orderedList);
            }

            attributes = null;
        }
    }
}