using System.ComponentModel.DataAnnotations;

namespace AsciidocNet
{
	public enum AttributeStyle
	{
		[Display(Name = "normal")] Normal,

		[Display(Name = "literal")] Literal,

		[Display(Name = "verse")] Verse,

		[Display(Name = "quote")] Quote,

		[Display(Name = "listing")] Listing,

		[Display(Name = "TIP")] Tip,

		[Display(Name = "NOTE")] Note,

		[Display(Name = "IMPORTANT")] Important,

		[Display(Name = "WARNING")] Warning,

		[Display(Name = "CAUTION")] Caution,

		[Display(Name = "abstract")] Abstract,

		[Display(Name = "partintro")] PartIntro,

		[Display(Name = "comment")] Comment,

		[Display(Name = "example")] Example,

		[Display(Name = "sidebar")] Sidebar,

		[Display(Name = "source")] Source,
	}
}