using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class Table : IElement, IAttributable, IList<TableColumn>
	{
		public Table()
		{
			// TODO: Copy the ctor info
		}

		private readonly List<TableColumn> _columns = new List<TableColumn>();

		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public TableColumnLayouts Layout { get; set; }

		public TableRowLayout Rows { get; private set; }

		public string Delimiter { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public void AssignColumnWidths(int widthBase)
		{
			var pf = Math.Pow(10, 4);
			var totalWidth = 0d;
			double? colPercentageWidth = 0d;

			if (widthBase > 0)
			{
				foreach (var column in this)
				{
					colPercentageWidth = column.AssignWidth(colPercentageWidth, widthBase, pf);
					totalWidth += colPercentageWidth.GetValueOrDefault();
				}
			}
			else
			{
				colPercentageWidth = ((100 * pf / this.Count) / pf);
				if (colPercentageWidth % 1 == 0)
				{
					colPercentageWidth = Convert.ToInt32(colPercentageWidth);
				}

				foreach (var column in this)
				{
					totalWidth += column.AssignWidth(colPercentageWidth) ?? 0;
				}
			}

			if (totalWidth != 100)
			{
				var lastColumn = this._columns.LastOrDefault();
				if (lastColumn != null)
				{
					colPercentageWidth = (Math.Round(100 - totalWidth + colPercentageWidth.GetValueOrDefault()) * pf) / pf;
					lastColumn.AssignWidth(colPercentageWidth);
				}
			}
		}

		public void CreateColumns()
		{
			if (this.Layout != null)
			{
				var widthBase = 0;
				foreach (var columnLayout in this.Layout)
				{
					widthBase += columnLayout.Width.GetValueOrDefault();
					this.Add(new TableColumn(this.Count, columnLayout));
				}

				if (this.Count > 0)
				{
					// TODO: Determine if we want to use an attribute for this
					this.Attributes.Add(new NamedAttribute("colcount", this.Count.ToString(), false));
					this.AssignColumnWidths(widthBase);
				}

				this.ExplicitColSpecs = true;
			}
		}

		public bool ExplicitColSpecs { get; private set; }

		public IEnumerator<TableColumn> GetEnumerator() => _columns.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(TableColumn item)
		{
			item.Parent = this;
			_columns.Add(item);
		}

		public void Clear()
		{
			foreach (var column in _columns)
			{
				column.Parent = null;
			}
			_columns.Clear();
		}

		public bool Contains(TableColumn item) => _columns.Contains(item);

		public void CopyTo(TableColumn[] array, int arrayIndex) => _columns.CopyTo(array, arrayIndex);

		public bool Remove(TableColumn item)
		{
			var remove = _columns.Remove(item);
			if (remove)
			{
				item.Parent = null;
			}
			return remove;
		}

		public int Count => _columns.Count;

		public bool IsReadOnly => false;

		public bool HasHeaderOption { get; set; }

		public int IndexOf(TableColumn item) => _columns.IndexOf(item);

		public void Insert(int index, TableColumn item)
		{
			item.Parent = this;
			_columns.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			var item = this[index];
			if (item != null)
			{
				item.Parent = null;
			}
			_columns.RemoveAt(index);
		}

		public TableColumn this[int index]
		{
			get { return _columns[index]; }
			set
			{
				value.Parent = this;
				_columns[index] = value;
			}
		}
	}

	public class TableRowLayout
	{
		public IList<TableRow> Head { get; }
		public IList<TableRow> Body { get; }
		public IList<TableRow> Footer { get; }

		public TableRowLayout()
		{
			Head = new List<TableRow>();
			Body = new List<TableRow>();
			Footer = new List<TableRow>();
		}
	}

	public class TableRow
	{
		
	}

	public class TableCell
	{
		private string cellText;
		private TableColumn column;

		public TableCell(TableColumn column, string cellText, TableCellLayout spec)
		{
			this.column = column;
			this.cellText = cellText;
			this.Layout = spec;
		}

		public TableCellLayout Layout { get; set; }
	}
}