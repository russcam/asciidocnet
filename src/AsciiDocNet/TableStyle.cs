using System.ComponentModel.DataAnnotations;

namespace AsciiDocNet
{
	public enum TableStyle
	{
		[Value("d")]
		None,

		[Value("s")]
		Strong,

		[Value("e")]
		Emphasis,

		[Value("m")]
		Monospaced,

		[Value("h")]
		Header,

		[Value("l")]
		Literal,

		[Value("v")]
		Verse,

		[Value("a")]
		AsciiDoc
	}
}