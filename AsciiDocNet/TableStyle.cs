using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
	public enum TableStyle
	{
		[Display(ShortName = "d")]
		None,

		[Display(ShortName = "s")]
		Strong,

		[Display(ShortName = "e")]
		Emphasis,

		[Display(ShortName = "m")]
		Monospaced,

		[Display(ShortName = "h")]
		Header,

		[Display(ShortName = "l")]
		Literal,

		[Display(ShortName = "v")]
		Verse,

		[Display(ShortName = "a")]
		Asciidoc
	}
}