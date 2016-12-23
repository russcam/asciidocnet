using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
    /// <summary>
    /// The table delimiter
    /// </summary>
    public enum TableDelimiters
	{
        /// <summary>
        /// Pipe separated
        /// </summary>
        [Display(ShortName = "|")]
		Psv,

        /// <summary>
        /// Colon separated
        /// </summary>
		[Display(ShortName = ":")]
		Dsv,

        /// <summary>
        /// Comma separated
        /// </summary>
		[Display(ShortName = ",")]
		Csv
	}
}