using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
	public enum TableDelimiters
	{
		[Display(ShortName = "|")]
		Psv,

		[Display(ShortName = ":")]
		Dsv,

		[Display(ShortName = ",")]
		Csv
	}
}