namespace AsciiDocNet
{
	public class TableCellLayout
	{
		public int? ColSpan { get; set; }
		public int? RowSpan { get; set; }

		public int? RepeatCol { get; set; }

		public TableHorizontalAlignment? HorizontalAlign { get; set; }

		public TableVerticalAlignment? VerticalAlign { get; set; }

		public TableStyle? Style { get; set; }
	}
}