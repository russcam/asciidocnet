using System;

namespace AsciiDocNet
{
	[Flags]
	public enum InlineElementType
	{
		Literal = 1 << 0,
		Emphasis = 1 << 1,
		EmphasisDouble = 1 << 2,
		Strong = 1 << 3,
		StrongDouble = 1 << 4,
		Monospace = 1 << 5,
		MonospaceDouble = 1 << 6,
		Subscript = 1 << 7,
		Superscript = 1 << 8,
		Quotation = 1 << 9,
		QuotationDouble = 1 << 10,
		Mark = 1 << 11,
		MarkDouble = 1 << 12,
		InternalAnchor = 1 << 13,
		InlineAnchor = 1 << 14,
		AttributeReference = 1 << 15,
		ImplicitLink = 1 << 16,
		All = Literal | Emphasis | EmphasisDouble | Strong | StrongDouble | Monospace | MonospaceDouble | Subscript | Superscript | Quotation | QuotationDouble | Mark | MarkDouble | InternalAnchor | InlineAnchor | AttributeReference | ImplicitLink
	}
}