using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class Filtering<TItem> : IFiltering<TItem>
    {
        public TreeRowNode<TItem> FilterBy(TreeRowNode<TItem> tree, 
                                           List<FilterModel> filters,
                                           List<GridColumn<TItem>> gridColumns, 
                                           CellParams<TItem> cellParams)
        {
            foreach (var child in tree.Children)
                Show(child, filters, gridColumns, cellParams);
            return tree;
        }

        private bool Show(TreeRowNode<TItem> tree, 
                          List<FilterModel> filters, 
                          List<GridColumn<TItem>> gridColumns,
                          CellParams<TItem> cellParams)
        {
            if (!tree.Data.IsGroup)
            {
                tree.Data.Show = true;
                foreach (var filter in filters)
                {
                    var gridColumn = gridColumns.Find(e => e.DataField == filter.DataDield);

                    cellParams.RowData = tree.Data.RowData;
                    cellParams.GridColumn = gridColumn;
                    cellParams.RowNode = tree.Data;

                    switch (gridColumn.CellDataType)
                    {
                        case CellDataType.Number:
                        case CellDataType.Boolean:
                        case CellDataType.Text:
                            if (gridColumn.CellDataFn(cellParams) is null || !gridColumn.CellDataFn(cellParams).ToString().ToLower().Contains(filter.Filter.ToLower()))
                            {
                                tree.Data.Show = false;
                                break;
                            }

                            break;
                        case CellDataType.Date:

                            if (!DateTime.TryParse(gridColumn.CellDataFn(cellParams).ToString(), out _) ||
                                !DateTime.TryParse(filter.Filter, out _))
                                return false;

                            //if (!BindConverter.TryConvertTo<DateTime>(gridColumn.CellDataFn(cellParams), CultureInfo.InvariantCulture, out _) ||
                            //    !DateTime.TryParse(filter.Filter, out _))
                            //    return false;

                            var columnData = Convert.ToDateTime(gridColumn.CellDataFn(cellParams)).ToString(gridColumn.DateFormat);

                            var columnFilter = Convert.ToDateTime(filter.Filter).ToString(gridColumn.DateFormat);

                            var result = columnData.CompareTo(columnFilter);

                            if (gridColumn.CellDataFn(cellParams) is null || result != 0)
                            {
                                tree.Data.Show = false;
                                break;
                            }

                            break;
                    }
                }
            }
            else
            {
                tree.Data.Show = false;
                foreach (var child in tree.Children)
                    if (Show(child, filters, gridColumns, cellParams))
                        tree.Data.Show = true;
            }

            return tree.Data.Show;
        }
    }
}