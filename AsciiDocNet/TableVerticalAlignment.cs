using System.ComponentModel.DataAnnotations;

namespace AsciidocNet
{
	public enum TableVerticalAlignment
	{
		[Display(ShortName = "<")]
		Top,

		[Display(ShortName = ">")]
		Bottom,

		[Display(ShortName = "^")]
		Middle
	}
}