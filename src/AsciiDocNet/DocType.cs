namespace AsciiDocNet
{
    /// <summary>
    /// The Document type
    /// </summary>
    public enum DocType
    {
        /// <summary>
        ///     Default doctype. In DocBook, includes the appendix, abstract, bibliography, glossary, and index sections
        /// </summary>
        Article,

        /// <summary>
        ///     The same as articles with the additional ability to use a top level title as part titles,
        ///     includes the appendix, dedication, preface, bibliography, glossary, index, and colophon.
        /// </summary>
        Book,

        /// <summary>
        ///     Only a single paragraph is read from the AsciiDoc source
        ///     Inline formatting is applied
        ///     The output is not wrapped in the normal paragraph tags
        /// </summary>
        Inline,

        /// <summary>
        ///     Special title and section naming conventions used to generate roff format UNIX manual pages. Corresponds to the
        ///     DocBook refentry document type.
        /// </summary>
        ManPage
    }
}