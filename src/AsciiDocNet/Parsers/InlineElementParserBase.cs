using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class InlineElementParserBase : IElementParser
    {
        public abstract void Parse(Container container, IDocumentReader reader, Func<string, bool> predicate,
            ref List<string> buffer,
            ref AttributeList attributes);

        private AttributeList ParseQuotedAttributes(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (value.IndexOf("{", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                // TODO: Handle attribute references. Do nothing for now, let them fall through
            }

            value = value.Trim();

            if (value.IndexOf(",", StringComparison.OrdinalIgnoreCase) > -1)
            {
                value = value.Split(new[] { ',' }, 2)[0];
            }

            if (value.Length == 0)
            {
                return new AttributeList();
            }

            if (value.StartsWith(".") || value.StartsWith("#"))
            {
                var segments = value.Split(new[] { '#' }, 2);
                string id = null;
                HashSet<string> roles = new HashSet<string>();
                if (segments.Length > 1)
                {
                    var subSegments = segments[1].Split('.');

                    if (subSegments.Length > 1)
                    {
                        id = subSegments[0];
                        for (int i = 1; i < subSegments.Length; i++)
                        {
                            roles.Add(subSegments[i]);
                        }
                    }
                }

                foreach (var role in segments[0].Split('.'))
                {
                    roles.Add(role);
                }

                var attributes = new AttributeList();
                if (id != null)
                {
                    attributes.Add(new IdAttribute(id, false));
                }
                if (roles.Any())
                {
                    attributes.Add(new RoleAttribute(roles, false));
                }

                return attributes;
            }

            return new AttributeList { new RoleAttribute(value, false) };
        }

        protected void ProcessParagraph(Container parent, ref List<string> buffer)
        {
            AttributeList attributes = null;
            ProcessParagraph(parent, ref buffer, ref attributes);
        }

        protected void ProcessParagraph(Container parent, ref List<string> buffer, ref AttributeList attributes)
        {
            if (buffer.Count > 0 && !buffer.All(string.IsNullOrWhiteSpace))
            {
                var inlineElements = ProcessInlineElements(string.Join(Environment.NewLine, buffer));
                var paragraph = new Paragraph(inlineElements);
                paragraph.Attributes.Add(attributes);
                parent.Add(paragraph);

                attributes = null;
                buffer = new List<string>(8);
            }
        }

        private InlineElementRuleMatch<TInlineElement> CreateContainerInlineElement<TInlineElement>(
            Match match,
            InlineElementConstraint constraint)
            where TInlineElement : InlineContainer, IInlineElement, IAttributable, new()
        {
            string unEscapedAttributes = null;
            var firstMatch = match.Groups[0].Value;

            if (firstMatch.StartsWith("\\"))
            {
                if (constraint == InlineElementConstraint.Constrained && !string.IsNullOrEmpty(match.Groups[2].Value))
                {
                    unEscapedAttributes = match.Groups[2].Value;
                }
                else
                {
                    var element = new TInlineElement();
                    firstMatch = firstMatch.Substring(1, firstMatch.Length - 1);
                    foreach (var inlineElement in ProcessInlineElements(firstMatch, element.ContainElementType))
                    {
                        element.Add(inlineElement);
                    }

                    return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length, new string(' ', match.Length));
                }
            }

            if (constraint == InlineElementConstraint.Constrained)
            {
                if (unEscapedAttributes != null)
                {
                    // TODO: Parse the unEscapedAttributes and add to element

                    var element = new TInlineElement();

                    foreach (var inlineElement in ProcessInlineElements(match.Groups[3].Value, element.ContainElementType))
                    {
                        element.Add(inlineElement);
                    }

                    var startIndex = match.Value[0] == ' ' || match.Value[0] == '\t' ? match.Index + 1 : match.Index;
                    var endIndex = match.Index + match.Length;
                    return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex, new string(' ', match.Length));
                }
                else
                {
                    var attributes = ParseQuotedAttributes(match.Groups[2].Value);
                    var element = new TInlineElement();

                    foreach (var inlineElement in ProcessInlineElements(match.Groups[3].Value, element.ContainElementType))
                    {
                        element.Add(inlineElement);
                    }

                    if (attributes != null)
                    {
                        element.Attributes.Add(attributes);
                    }

	                var group1 = match.Groups[1].Value;

                    var startIndex = !string.IsNullOrEmpty(group1) 
	                    ? match.Index + group1.Length 
	                    : match.Index;
	                
                    var endIndex = match.Index + match.Length;

                    //TODO: do something with match.Groups[1].Value
	                var replacement = !string.IsNullOrEmpty(group1)
		                ? group1 + new string(' ', match.Length - group1.Length)
		                : new string(' ', match.Length);
	                
                    return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex, replacement);
                }
            }
            else
            {
                var element = new TInlineElement();

                foreach (var inlineElement in ProcessInlineElements(match.Groups[2].Value, element.ContainElementType))
                {
                    element.Add(inlineElement);
                }

                var attributes = ParseQuotedAttributes(match.Groups[1].Value);
                if (attributes != null)
                {
                    element.Attributes.Add(attributes);
                }

                return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length, new string(' ', match.Length));
            }
        }

        private InlineElementRuleMatch<TInlineElement> CreateInlineElement<TInlineElement>(Match match, InlineElementConstraint constraint)
            where TInlineElement : IInlineElement, IText, IAttributable, new()
        {
            string unEscapedAttributes = null;
            var firstMatch = match.Groups[0].Value;

            if (firstMatch.StartsWith("\\"))
            {
                if (constraint == InlineElementConstraint.Constrained && !string.IsNullOrEmpty(match.Groups[2].Value))
                {
                    unEscapedAttributes = match.Groups[2].Value;
                }
                else
                {
                    var element = new TInlineElement
                    {
                        Text = firstMatch.Substring(1, firstMatch.Length - 1)
                    };

                    return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length, new string(' ', match.Length));
                }
            }

            if (constraint == InlineElementConstraint.Constrained)
            {
                if (unEscapedAttributes != null)
                {
                    // TODO: Parse the unEscapedAttributes and add to element

                    var element = new TInlineElement
                    {
                        Text = match.Groups[3].Value
                    };

                    var startIndex = match.Value[0] == ' ' || match.Value[0] == '\t' 
	                    ? match.Index + 1 
	                    : match.Index;
	                
                    var endIndex = match.Index + match.Length;
                    return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex, new string(' ', match.Length));
                }
                else
                {
                    var attributes = ParseQuotedAttributes(match.Groups[2].Value);

                    var element = new TInlineElement
                    {
                        Text = match.Groups[3].Value
                    };

                    if (attributes != null)
                    {
                        element.Attributes.Add(attributes);
                    }

	                var group1 = match.Groups[1].Value;

	                var startIndex = !string.IsNullOrEmpty(group1) 
		                ? match.Index + group1.Length 
		                : match.Index;
	                
	                var endIndex = match.Index + match.Length;

	                //TODO: do something with match.Groups[1].Value
	                var replacement = !string.IsNullOrEmpty(group1)
		                ? group1 + new string(' ', match.Length - group1.Length)
		                : new string(' ', match.Length);
	                
                    return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex, replacement);
                }
            }
            else
            {
                var element = new TInlineElement
                {
                    Text = match.Groups[2].Value
                };

                var attributes = ParseQuotedAttributes(match.Groups[1].Value);
                if (attributes != null)
                {
                    element.Attributes.Add(attributes);
                }

                return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length, new string(' ', match.Length));
            }
        }


        protected IList<IInlineElement> ProcessInlineElements(string block, InlineElementType elementType = InlineElementType.All)
        {
            var elements = new List<IInlineElement>();
            if (string.IsNullOrWhiteSpace(block))
            {
                return elements;
            }

            var matches = new List<InlineElementRuleMatch>();

            foreach (var rule in PatternMatcher.InlineElementRules.Where(r => elementType.HasFlag(r.ElementType)))
            {
                if (rule.Regex.IsMatch(block))
                {
                    var currentRule = rule;
                    var outerMatches = matches;
                    block = currentRule.Regex.Replace(block, match =>
                    {
                        switch (currentRule.ElementType)
                        {
                            case InlineElementType.Emphasis:
                            case InlineElementType.EmphasisDouble:
                                var emphasis = CreateInlineElement<Emphasis>(match, currentRule.Constraint);

                                if (currentRule.ElementType == InlineElementType.EmphasisDouble)
                                {
                                    emphasis.Element.DoubleDelimited = true;
                                }

                                outerMatches.Add(emphasis);
                                break;
                            case InlineElementType.Strong:
                            case InlineElementType.StrongDouble:
                                var strong = CreateContainerInlineElement<Strong>(match, currentRule.Constraint);

                                if (currentRule.ElementType == InlineElementType.StrongDouble)
                                {
                                    strong.Element.DoubleDelimited = true;
                                }

                                outerMatches.Add(strong);
                                break;
                            case InlineElementType.Monospace:
                            case InlineElementType.MonospaceDouble:
                                var monospace = CreateContainerInlineElement<Monospace>(match, currentRule.Constraint);
                                if (currentRule.ElementType == InlineElementType.MonospaceDouble)
                                {
                                    monospace.Element.DoubleDelimited = true;
                                }

                                outerMatches.Add(monospace);
                                break;
                            case InlineElementType.Subscript:
                                var subscript = CreateInlineElement<Subscript>(match, currentRule.Constraint);
                                outerMatches.Add(subscript);

                                break;
                            case InlineElementType.Superscript:
                                var superscript = CreateInlineElement<Superscript>(match, currentRule.Constraint);
                                outerMatches.Add(superscript);

                                break;
                            case InlineElementType.Quotation:
                            case InlineElementType.QuotationDouble:
                                var quotation = CreateContainerInlineElement<QuotationMark>(match, currentRule.Constraint);
                                if (currentRule.ElementType == InlineElementType.QuotationDouble)
                                {
                                    quotation.Element.DoubleDelimited = true;
                                }

                                outerMatches.Add(quotation);
                                break;
                            case InlineElementType.Mark:
                            case InlineElementType.MarkDouble:
                                var mark = CreateContainerInlineElement<Mark>(match, currentRule.Constraint);

                                if (currentRule.ElementType == InlineElementType.MarkDouble)
                                {
                                    mark.Element.DoubleDelimited = true;
                                }

                                outerMatches.Add(mark);
                                break;
                            case InlineElementType.InternalAnchor:
                                var internalAnchorParts = match.Groups[1].Value;
                                InternalAnchor anchor;
                                if (internalAnchorParts.IndexOf(",", StringComparison.OrdinalIgnoreCase) > -1)
                                {
                                    var parts = internalAnchorParts.Split(',');
                                    // TODO: Process inline elements of parts[1]
                                    anchor = new InternalAnchor(parts[0], parts[1]);
                                }
                                else
                                {
                                    anchor = new InternalAnchor(internalAnchorParts);
                                }

                                outerMatches.Add(new InlineElementRuleMatch(anchor, match.Index, match.Index + match.Length, new string(' ', match.Length)));
                                break;
                            case InlineElementType.InlineAnchor:
                                var inlineAnchorId = match.Groups[1].Value;
                                var xRefLabel = match.Groups[2].Value;

                                // TODO: process inline elements of xRefLabel
                                var inlineAnchor = !string.IsNullOrEmpty(xRefLabel)
                                    ? new InlineAnchor(inlineAnchorId, xRefLabel)
                                    : new InlineAnchor(inlineAnchorId);

                                outerMatches.Add(new InlineElementRuleMatch(inlineAnchor, match.Index, match.Index + match.Length, new string(' ', match.Length)));
                                break;
                            case InlineElementType.AttributeReference:
                                var text = match.Groups[2].Value;
                                var attributeReference = new AttributeReference(text);
                                outerMatches.Add(new InlineElementRuleMatch(attributeReference, match.Index, match.Index + match.Length, new string(' ', match.Length)));
                                break;
                            case InlineElementType.ImplicitLink:
                                // TODO: split attributes out
	                            var attributes = match.Groups["attributes"].Success
		                            ? match.Groups["attributes"].Value
		                            : null;
                                var href = match.Groups[2].Value;
                                var link = new Link(href, attributes);
                                outerMatches.Add(new InlineElementRuleMatch(link, match.Index + match.Groups[1].Length, match.Index + match.Length, match.Groups[1].Value + new string(' ', match.Length - match.Groups[1].Length)));
                                break;
                        }

	                    // replace any matches with the match replacement value TODO: This could be optimized
	                    return outerMatches[outerMatches.Count - 1].Replacement;
                    });
                }
            }

            if (matches.Any())
            {
                matches.Sort((match1, match2) => match1.StartIndex.CompareTo(match2.StartIndex));
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    var isLastMatch = i == matches.Count - 1;

                    if (i == 0 && match.StartIndex > 0)
                    {
                        elements.Add(new TextLiteral(block.Substring(0, matches[0].StartIndex)));
                    }

                    elements.Add(match.Element);

                    if (!isLastMatch)
                    {
                        // is there a literal between where this match ends and next one begins
                        var nextMatch = matches[i + 1];
                        var length = nextMatch.StartIndex - match.EndIndex;
                        if (length > 0)
                        {
                            elements.Add(new TextLiteral(block.Substring(match.EndIndex, length)));
                        }
                    }
                    else
                    {
                        if (match.EndIndex < block.Length)
                        {
                            elements.Add(new TextLiteral(block.Substring(match.EndIndex)));
                        }
                    }
                }
            }
            else
            {
                elements.Add(new TextLiteral(block));
            }

            return elements;
        }

    }
}