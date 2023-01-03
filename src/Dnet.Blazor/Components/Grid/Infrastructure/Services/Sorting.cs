using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class Sorting<TItem> : ISorting<TItem>
    {
        public void UpdateOrder(List<GridColumn<TItem>> gridColumns, GridColumn<TItem> gridColumn)
        {
            var sortStatus = gridColumn.SortStatus;

            gridColumns.ForEach(p => p.SortStatus = SortOrder.None);

            gridColumn.SortStatus = sortStatus switch
            {
                SortOrder.None => SortOrder.Ascending,
                SortOrder.Ascending => SortOrder.Descending,
                SortOrder.Descending => SortOrder.None,
                _ => gridColumn.SortStatus
            };
        }

        private class NumberCell
        {
            public long Value { get; set; }

            public int SortOrder { get; set; }

            public int Position { get; set; }
        }

        private int NumberCellComparer(NumberCell a, NumberCell b)
        {
            return a.Value.CompareTo(b.Value) * a.SortOrder;
        }

        private class DecimalNumberCell
        {
            public decimal Value { get; set; }

            public int SortOrder { get; set; }

            public int Position { get; set; }
        }

        private int DecimalNumberCellComparer(DecimalNumberCell a, DecimalNumberCell b)
        {
            return a.Value.CompareTo(b.Value) * a.SortOrder;
        }

        private List<TreeRowNode<TItem>> SortNumbers(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {
            if (gridColumn.IsDecimalCellDataType)
            {
                List<DecimalNumberCell> numberCells = new();

                cellParams.GridColumn = gridColumn;

                for (var i = 0; i < nodes.Count; ++i)
                {
                    cellParams.RowData = nodes[i].Data.RowData;

                    var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                    var dataString = "";

                    if (data != null)
                    {
                        dataString = data.ToString().Replace(',', '.');
                    }

                    var numberCell = new DecimalNumberCell
                    {
                        Position = i,
                        SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                        Value = data is null ? decimal.MinValue : decimal.Parse(dataString)
                    };

                    numberCells.Add(numberCell);
                }

                numberCells.Sort(DecimalNumberCellComparer);

                List<TreeRowNode<TItem>> sortedChildren = new();

                foreach (var numberCell in numberCells)
                    sortedChildren.Add(nodes[numberCell.Position]);

                return sortedChildren;
            }
            else
            {
                List<NumberCell> numberCells = new();

                cellParams.GridColumn = gridColumn;

                for (var i = 0; i < nodes.Count; ++i)
                {
                    cellParams.RowData = nodes[i].Data.RowData;

                    var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                    var numberCell = new NumberCell
                    {
                        Position = i,
                        SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                        Value = data is null ? long.MinValue : long.Parse(data.ToString())
                    };

                    numberCells.Add(numberCell);
                }

                numberCells.Sort(NumberCellComparer);

                List<TreeRowNode<TItem>> sortedChildren = new();

                foreach (var numberCell in numberCells)
                    sortedChildren.Add(nodes[numberCell.Position]);

                return sortedChildren;
            }
        }

        private class TextCell
        {
            public string Value { get; set; }
            public int SortOrder { get; set; }
            public int Position { get; set; }
        }

        private int TextCellComparer(TextCell a, TextCell b)
        {
            return a.Value.CompareTo(b.Value) * a.SortOrder;
        }

        private List<TreeRowNode<TItem>> SortTexts(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {
            List<TextCell> textCells = new();
            
            cellParams.GridColumn = gridColumn;

            for (var i = 0; i < nodes.Count; ++i)
            {
                cellParams.RowData = nodes[i].Data.RowData;
                
                var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                var textCell = new TextCell
                {
                    Position = i, 
                    SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                    Value = data is null ? string.Empty : data.ToString()
                };

                textCells.Add(textCell);
            }

            textCells.Sort(TextCellComparer);

            List<TreeRowNode<TItem>> sortedChildren = new();

            foreach (var textCell in textCells)
                sortedChildren.Add(nodes[textCell.Position]);

            return sortedChildren;
        }

        private class DateCell
        {
            public DateTime Value { get; set; }
            public int SortOrder { get; set; }
            public int Position { get; set; }
        }

        private int DateCellComparer(DateCell a, DateCell b)
        {
            return a.Value.CompareTo(b.Value) * a.SortOrder;
        }

        private List<TreeRowNode<TItem>> SortDates(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {
            List<DateCell> dateCells = new();

            cellParams.GridColumn = gridColumn;

            for (var i = 0; i < nodes.Count; ++i)
            {
                cellParams.RowData = nodes[i].Data.RowData;
                
                var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                var dateCell = new DateCell
                {
                    Position = i, 
                    SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                    Value = data is null ? DateTime.MinValue : DateTime.Parse(data.ToString())
                };

                dateCells.Add(dateCell);
            }

            dateCells.Sort(DateCellComparer);

            List<TreeRowNode<TItem>> sortedChildren = new();

            foreach (var dateCell in dateCells)
                sortedChildren.Add(nodes[dateCell.Position]);

            return sortedChildren;
        }

        private class BooleanCell
        {
            public bool Value { get; set; }
            public int SortOrder { get; set; }
            public int Position { get; set; }
        }

        private int BooleanCellComparer(BooleanCell a, BooleanCell b)
        {
            return a.Value.CompareTo(b.Value) * a.SortOrder;
        }

        private List<TreeRowNode<TItem>> SortBooleans(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {
            List<BooleanCell> booleanCells = new();

            cellParams.GridColumn = gridColumn;

            for (var i = 0; i < nodes.Count; ++i)
            {
                cellParams.RowData = nodes[i].Data.RowData;
                    
                var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                var booleanCell = new BooleanCell
                {
                    Position = i, 
                    SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                    Value = data is not null && bool.Parse(data.ToString())
                };

                booleanCells.Add(booleanCell);
            }

            booleanCells.Sort(BooleanCellComparer);

            List<TreeRowNode<TItem>> sortedChildren = new();

            foreach (var booleanCell in booleanCells)
                sortedChildren.Add(nodes[booleanCell.Position]);

            return sortedChildren;
        }

        public TreeRowNode<TItem> SortBy(TreeRowNode<TItem> tree, GridColumn<TItem> gridColumn, CellParams<TItem> cellParams)
        {
            if (tree.Children.Count > 0 && !tree.Children[0].Data.IsGroup)
            {
                tree.Children = gridColumn.CellDataType switch
                {
                    CellDataType.Number => SortNumbers(tree.Children, gridColumn, false, cellParams),
                    CellDataType.Text => SortTexts(tree.Children, gridColumn, false, cellParams),
                    CellDataType.Date => SortDates(tree.Children, gridColumn, false, cellParams),
                    CellDataType.Boolean => SortBooleans(tree.Children, gridColumn, false, cellParams),
                    _ => tree.Children
                };
            }
            else
            {
                foreach (var child in tree.Children)
                    SortBy(child, gridColumn, cellParams);
            }

            return tree;
        }


        public TreeRowNode<TItem> SortGroupingBy(TreeRowNode<TItem> tree, GridColumn<TItem> gridColumn, CellParams<TItem> cellParams)
        {
            if (tree.Children.Count > 0 && tree.Children[0].ColumnName == gridColumn.DataField)
            {
                tree.Children = gridColumn.CellDataType switch
                {
                    CellDataType.Number => SortNumbers(tree.Children, gridColumn, true, cellParams),
                    CellDataType.Text => SortTexts(tree.Children, gridColumn, true, cellParams),
                    CellDataType.Date => SortDates(tree.Children, gridColumn, true, cellParams),
                    CellDataType.Boolean => SortBooleans(tree.Children, gridColumn, true, cellParams),
                    _ => tree.Children
                };
            }
            else
            {
                foreach (var child in tree.Children)
                    SortGroupingBy(child, gridColumn, cellParams);
            }
            return tree;
        }
    }
}
