using System.ComponentModel.DataAnnotations;

namespace AsciidocNet
{
	public enum AdmonitionStyle
	{
		[Display(Name = "TIP")]
		Tip,

		[Display(Name = "NOTE")]
		Note,

		[Display(Name = "IMPORTANT")]
		Important,

		[Display(Name = "WARNING")]
		Warning,

		[Display(Name = "CAUTION")]
		Caution,
	}
}