using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
    /// <summary>
    /// The horizontal table alignment
    /// </summary>
    public enum TableHorizontalAlignment
	{
        /// <summary>
        /// left
        /// </summary>
        [Display(ShortName = "<")]
		Left,

        /// <summary>
        /// right
        /// </summary>
        [Display(ShortName = ">")]
		Right,

        /// <summary>
        /// center
        /// </summary>
        [Display(ShortName = "^")]
		Center
	}
}