using System;

namespace AsciiDocNet
{
    /// <summary>
    /// The inline element types
    /// </summary>
    [Flags]
    public enum InlineElementType
    {
        /// <summary>
        /// literal
        /// </summary>
        Literal = 1 << 0,
        /// <summary>
        /// emphasis
        /// </summary>
        Emphasis = 1 << 1,
        /// <summary>
        /// emphasis double
        /// </summary>
        EmphasisDouble = 1 << 2,
        /// <summary>
        /// strong
        /// </summary>
        Strong = 1 << 3,
        /// <summary>
        /// strong double
        /// </summary>
        StrongDouble = 1 << 4,
        /// <summary>
        /// monospace
        /// </summary>
        Monospace = 1 << 5,
        /// <summary>
        /// monospace double
        /// </summary>
        MonospaceDouble = 1 << 6,
        /// <summary>
        /// subscript
        /// </summary>
        Subscript = 1 << 7,
        /// <summary>
        /// superscript
        /// </summary>
        Superscript = 1 << 8,
        /// <summary>
        /// quotation
        /// </summary>
        Quotation = 1 << 9,
        /// <summary>
        /// quotation double
        /// </summary>
        QuotationDouble = 1 << 10,
        /// <summary>
        /// mark
        /// </summary>
        Mark = 1 << 11,
        /// <summary>
        /// mark double
        /// </summary>
        MarkDouble = 1 << 12,
        /// <summary>
        /// internal anchor
        /// </summary>
        InternalAnchor = 1 << 13,
        /// <summary>
        /// inline anchor
        /// </summary>
        InlineAnchor = 1 << 14,
        /// <summary>
        /// attribute reference
        /// </summary>
        AttributeReference = 1 << 15,
        /// <summary>
        /// implicit link
        /// </summary>
        ImplicitLink = 1 << 16,
        /// <summary>
        /// All elements
        /// </summary>
        All = Literal | Emphasis | EmphasisDouble | Strong | StrongDouble | Monospace | MonospaceDouble | Subscript | Superscript | Quotation | QuotationDouble | Mark | MarkDouble | InternalAnchor | InlineAnchor | AttributeReference | ImplicitLink
    }
}