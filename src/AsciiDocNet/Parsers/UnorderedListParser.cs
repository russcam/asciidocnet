using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class UnorderedListParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.ListItem.IsMatch(reader.Line);

        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.ListItem.Match(reader.Line);
            if (!match.Success)
            {
                throw new ArgumentException("not an unordered list item");
            }

            var level = match.Groups["level"].Value;
            var text = match.Groups["text"].Value;
            var listItem = new UnorderedListItem(level.Length);
            listItem.Attributes.Add(attributes);

            buffer.Add(text);
            reader.ReadLine();

	        attributes = null;
	        
            while (reader.Line != null &&
                   !PatternMatcher.BlankCharacters.IsMatch(reader.Line) &&
                   !PatternMatcher.ListItem.IsMatch(reader.Line) &&
                   !PatternMatcher.CheckListItem.IsMatch(reader.Line) &&
                   (predicate == null || !predicate(reader.Line)))
            {
	            if (PatternMatcher.ListItemContinuation.IsMatch(reader.Line))
	            {
		            ProcessBuffer(listItem, ref buffer, ref attributes);	            
		            reader.ReadLine();
		            DescendingParse(
			            listItem, 
			            reader, 
			            line => PatternMatcher.BlankCharacters.IsMatch(line) || 
			                    PatternMatcher.ListItem.IsMatch(line) ||
			                    PatternMatcher.CheckListItem.IsMatch(reader.Line), 
			            ref buffer, 
			            ref attributes);
	            }
	            else
	            {
		            buffer.Add(reader.Line);
		            reader.ReadLine();
	            }
            }

	        ProcessBuffer(listItem, ref buffer, ref attributes);

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