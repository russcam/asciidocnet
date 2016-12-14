using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
	public enum TableHorizontalAlignment
	{
		[Display(ShortName = "<")]
		Left,

		[Display(ShortName = ">")]
		Right,

		[Display(ShortName = "^")]
		Center
	}
}