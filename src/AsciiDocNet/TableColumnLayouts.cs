using System.Collections;
using System.Collections.Generic;

namespace AsciiDocNet
{
	public class TableColumnLayouts : List<TableColumnLayout>
	{
	}

	public class TableColumnLayout
	{
		public TableVerticalAlignment? VerticalAlign { get; set; }

		public TableHorizontalAlignment? HorizontalAlign { get; set; }

		public int? Width { get; set; } 

		public TableStyle? Style { get; set; }

		public double? ColPercentageWidth { get; set; }

		public double? ColAbsWidth { get; set; }
	}
}