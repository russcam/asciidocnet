using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
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