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

        private class GenericCell<T>
        {
            public T? Value { get; set; }
            public int SortOrder { get; set; }
            public int Position { get; set; }

             public void SetValue(T value)
             {
                Value = value;
             }
        }

        private class GenericCellComparer<T> : IComparer<GenericCell<T>> where T : IComparable<T>
        {
            public int Compare(GenericCell<T> x, GenericCell<T> y)
            {
                int result = x.Value.CompareTo(y.Value);
                return result * x.SortOrder;
            }
        }

        private List<TreeRowNode<TItem>> SortNumbers(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {

            if (gridColumn.IsDecimalCellDataType)
            {
                List<GenericCell<decimal>> numberCells = new();

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

                    var numberCell = new GenericCell<decimal>
                    {
                        Position = i,
                        SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                        Value = data is null ? decimal.MinValue : decimal.Parse(dataString)
                    };

                    numberCells.Add(numberCell);
                }

                var cellComparer = new GenericCellComparer<decimal>();
                numberCells.Sort(cellComparer);

                List<TreeRowNode<TItem>> sortedChildren = new();

                foreach (var numberCell in numberCells)
                    sortedChildren.Add(nodes[numberCell.Position]);

                return sortedChildren;
            }
            else
            {
                List<GenericCell<long>> numberCells = new();

                cellParams.GridColumn = gridColumn;

                for (var i = 0; i < nodes.Count; ++i)
                {
                    cellParams.RowData = nodes[i].Data.RowData;

                    var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                    var numberCell = new GenericCell<long>
                    {
                        Position = i,
                        SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                        Value = data is null ? long.MinValue : long.Parse(data.ToString())
                    };

                    numberCells.Add(numberCell);
                }

                var cellComparer = new GenericCellComparer<long>();
                numberCells.Sort(cellComparer);

                List<TreeRowNode<TItem>> sortedChildren = new();

                foreach (var numberCell in numberCells)
                    sortedChildren.Add(nodes[numberCell.Position]);

                return sortedChildren;
            }
        }

        private List<TreeRowNode<TItem>> SortTexts(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {
            List<GenericCell<string>> textCells = new();

            cellParams.GridColumn = gridColumn;

            for (var i = 0; i < nodes.Count; ++i)
            {
                cellParams.RowData = nodes[i].Data.RowData;

                var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                var textCell = new GenericCell<string>
                {
                    Position = i,
                    SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                    Value = data is null ? string.Empty : data.ToString()
                };

                textCells.Add(textCell);
            }

            var cellComparer = new GenericCellComparer<string>();
            textCells.Sort(cellComparer);

            List<TreeRowNode<TItem>> sortedChildren = new();

            foreach (var textCell in textCells)
                sortedChildren.Add(nodes[textCell.Position]);

            return sortedChildren;
        }


        private List<TreeRowNode<TItem>> SortDates(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {
            List<GenericCell<DateTime>> dateCells = new();

            cellParams.GridColumn = gridColumn;

            for (var i = 0; i < nodes.Count; ++i)
            {
                cellParams.RowData = nodes[i].Data.RowData;

                var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                var dateCell = new GenericCell<DateTime>
                {
                    Position = i,
                    SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                    Value = data is null ? DateTime.MinValue : DateTime.Parse(data.ToString())
                };

                dateCells.Add(dateCell);
            }

            var cellComparer = new GenericCellComparer<DateTime>();
            dateCells.Sort(cellComparer);

            List<TreeRowNode<TItem>> sortedChildren = new();

            foreach (var dateCell in dateCells)
                sortedChildren.Add(nodes[dateCell.Position]);

            return sortedChildren;
        }

        private List<TreeRowNode<TItem>> SortBooleans(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
        {
            List<GenericCell<bool>> booleanCells = new();

            cellParams.GridColumn = gridColumn;

            for (var i = 0; i < nodes.Count; ++i)
            {
                cellParams.RowData = nodes[i].Data.RowData;

                var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);

                var booleanCell = new GenericCell<bool>
                {
                    Position = i,
                    SortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1,
                    Value = data is not null && bool.Parse(data.ToString())
                };

                booleanCells.Add(booleanCell);
            }

            var cellComparer = new GenericCellComparer<bool>();
            booleanCells.Sort(cellComparer);

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
