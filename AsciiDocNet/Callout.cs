namespace AsciiDocNet
{
	public class Callout
	{
		public int Number { get; set; }

		public string Text { get; set; }

		public Callout(int number, string text)
		{
			Number = number;
			Text = text;
		}
	}
}