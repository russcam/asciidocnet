using System.ComponentModel.DataAnnotations;

namespace AsciidocNet
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