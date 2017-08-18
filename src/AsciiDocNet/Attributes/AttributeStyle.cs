using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
    /// <summary>
    /// The style of attribute
    /// </summary>
    public enum AttributeStyle
	{
        /// <summary>
        /// normal
        /// </summary>
        [Display(Name = "normal")] Normal,

        /// <summary>
        /// literal
        /// </summary>
        [Display(Name = "literal")] Literal,

        /// <summary>
        /// verse
        /// </summary>
        [Display(Name = "verse")] Verse,

        /// <summary>
        /// quote
        /// </summary>
        [Display(Name = "quote")] Quote,

        /// <summary>
        /// listing
        /// </summary>
        [Display(Name = "listing")] Listing,

        /// <summary>
        /// tip
        /// </summary>
        [Display(Name = "TIP")] Tip,

        /// <summary>
        /// note
        /// </summary>
        [Display(Name = "NOTE")] Note,

        /// <summary>
        /// important
        /// </summary>
        [Display(Name = "IMPORTANT")] Important,

        /// <summary>
        /// warning
        /// </summary>
        [Display(Name = "WARNING")] Warning,

        /// <summary>
        /// caution
        /// </summary>
        [Display(Name = "CAUTION")] Caution,

        /// <summary>
        /// abstract
        /// </summary>
        [Display(Name = "abstract")] Abstract,

        /// <summary>
        /// part intro
        /// </summary>
        [Display(Name = "partintro")] PartIntro,

        /// <summary>
        /// comment
        /// </summary>
        [Display(Name = "comment")] Comment,

        /// <summary>
        /// example
        /// </summary>
        [Display(Name = "example")] Example,

        /// <summary>
        /// sidebar
        /// </summary>
        [Display(Name = "sidebar")] Sidebar,

        /// <summary>
        /// source
        /// </summary>
        [Display(Name = "source")] Source,
	}
}