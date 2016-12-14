using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
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