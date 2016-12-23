using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
    /// <summary>
    /// The table style
    /// </summary>
    public enum TableStyle
    {
        /// <summary>
        /// none
        /// </summary>
        [Display(ShortName = "d")]
        None,

        /// <summary>
        /// strong
        /// </summary>
        [Display(ShortName = "s")]
        Strong,

        /// <summary>
        /// emphasis
        /// </summary>
        [Display(ShortName = "e")]
        Emphasis,

        /// <summary>
        /// monospaced
        /// </summary>
        [Display(ShortName = "m")]
        Monospaced,

        /// <summary>
        /// header
        /// </summary>
        [Display(ShortName = "h")]
        Header,

        /// <summary>
        /// literal
        /// </summary>
        [Display(ShortName = "l")]
        Literal,

        /// <summary>
        /// verse
        /// </summary>
        [Display(ShortName = "v")]
        Verse,

        /// <summary>
        /// asciidoc
        /// </summary>
        [Display(ShortName = "a")]
        Asciidoc
    }
}