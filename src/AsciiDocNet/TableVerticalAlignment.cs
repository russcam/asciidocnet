using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
    /// <summary>
    /// The vertical alignment for a table
    /// </summary>
    public enum TableVerticalAlignment
    {
        /// <summary>
        /// The top
        /// </summary>
        [Display(ShortName = "<")]
        Top,

        /// <summary>
        /// The bottom
        /// </summary>
        [Display(ShortName = ">")]
        Bottom,

        /// <summary>
        /// The middle
        /// </summary>
        [Display(ShortName = "^")]
        Middle
    }
}