using System;

namespace AsciiDocNet
{
	public class TableColumn : IAttributable
	{
		private readonly int _count;

		public TableColumn(int count, TableColumnLayout columnLayout)
		{
			_count = count;
			Layout = columnLayout;
		}

		public TableColumnLayout Layout { get; }

		public AttributeList Attributes { get; } = new AttributeList();

		public Table Parent { get; set; }

		public double? AssignWidth(double? colPercentageWidth, int? widthBase = null, double pf = 10000.0)
		{
			if (widthBase.HasValue && this.Layout.Width.HasValue)
			{
				colPercentageWidth = ((double)Layout.Width / widthBase.Value * 100 * pf) / pf;
				if (colPercentageWidth % 1 == 0)
				{
					colPercentageWidth = Convert.ToInt32(colPercentageWidth);
				}
			}

			this.Layout.ColPercentageWidth = colPercentageWidth;
			//this.Attributes.Add(new NamedAttribute("colpcwidth", colPercentageWidth.ToString(), false));

			var tableAbsWidth = this.Parent.Attributes["tableabswidth"] as NamedAttribute;
			if (tableAbsWidth != null)
			{
				var colAbsWidth = (colPercentageWidth / 100) * Math.Round(double.Parse(tableAbsWidth.Value));
				this.Layout.ColAbsWidth = colAbsWidth;
				//this.Attributes.Add(new NamedAttribute("colabswidth", colAbsWidth.ToString(), false));
			}

			return colPercentageWidth;
		}
	}
}