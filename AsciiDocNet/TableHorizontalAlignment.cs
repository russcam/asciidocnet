using System.ComponentModel.DataAnnotations;

namespace AsciidocNet
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