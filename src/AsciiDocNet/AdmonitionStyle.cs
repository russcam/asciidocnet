using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
    /// <summary>
    /// An Adomonition style
    /// </summary>
    public enum AdmonitionStyle
    {
        /// <summary>
        /// A tip admonition
        /// </summary>
        [Display(Name = "TIP")]
        Tip,

        /// <summary>
        /// A note admonition
        /// </summary>
        [Display(Name = "NOTE")]
        Note,

        /// <summary>
        /// A important admonition
        /// </summary>
        [Display(Name = "IMPORTANT")]
        Important,

        /// <summary>
        /// A warning admonition
        /// </summary>
        [Display(Name = "WARNING")]
        Warning,

        /// <summary>
        /// A caution admonition
        /// </summary>
        [Display(Name = "CAUTION")]
        Caution,
    }
}