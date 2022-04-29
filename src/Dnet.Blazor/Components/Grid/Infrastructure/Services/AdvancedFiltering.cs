using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Infrastructure.Models.SearchModels;
using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class AdvancedFiltering<TItem> : IAdvancedFiltering<TItem>
    {
        public List<GridColumn<TItem>> InitAdvancedFilterModels(List<GridColumn<TItem>> gridColumns, FilterOperator defaultAdvancedFilterOperator)
        {
            foreach (var gridColumn in gridColumns) 
            { 
                if (gridColumn.AdvancedFilterModel.Column is null && gridColumn.CellDataType != CellDataType.None && gridColumn.EnableAdvancedFilter)
                {
                    gridColumn.AdvancedFilterModel = new AdvancedFilterModel
                    {
                        Operator = FilterOperator.None,
                        AdditionalOperator = FilterOperator.None,
                        Type = gridColumn.CellDataType,
                        Column = gridColumn.DataField,
                        Value = "",
                        AdditionalValue = ""
                    };
                }

                if (gridColumn.CellDataType == CellDataType.Text)
                {
                    if (gridColumn.AdvancedFilterModel.Operator == FilterOperator.None)
                        gridColumn.AdvancedFilterModel.Operator = defaultAdvancedFilterOperator;
                }
            }

            return gridColumns;
        }

        public TreeRowNode<TItem> FilterBy(TreeRowNode<TItem> tree, 
                                           List<AdvancedFilterModel> advancedFilterList, 
                                           List<GridColumn<TItem>> gridColumns, 
                                           CellParams<TItem> cellParams)
        {
            foreach (var child in tree.Children)
                Show(child, advancedFilterList, gridColumns, cellParams);
            return tree;
        }

        private bool IsValidModelFirstPart(AdvancedFilterModel filterModel)
        {
            if (filterModel.Operator == FilterOperator.None && filterModel.Type != CellDataType.Boolean) return false;
            return true;
        }

        private bool IsValidModelSecondPart(AdvancedFilterModel filterModel)
        {
            if (filterModel.AdditionalOperator == FilterOperator.None && filterModel.Type != CellDataType.Boolean) return false;
            return true;
        }

        private bool Check(TreeRowNode<TItem> tree, 
                           string filterValue, 
                           FilterOperator filterOperator, 
                           AdvancedFilterModel filterModel, 
                           List<GridColumn<TItem>> gridColumns,
                           CellParams<TItem> cellParams)
        {
            var gridColumn = gridColumns.Find(e => e.DataField == filterModel.Column);

            cellParams.RowData = tree.Data.RowData;
            cellParams.GridColumn = gridColumn;
            cellParams.RowNode = tree.Data;
            
            if (gridColumn != null && gridColumn.CellDataFn(cellParams) is null) return false;

            var cellData = gridColumn.CellDataFn(cellParams).ToString();

            if (filterModel.Type == CellDataType.Text)
            {
                cellData = cellData.ToLower();

                filterValue = filterValue.ToLower();

                return filterOperator switch
                {
                    FilterOperator.Contains => cellData.Contains(filterValue),

                    FilterOperator.Equals => (cellData == filterValue),

                    FilterOperator.NotContains => !cellData.Contains(filterValue),

                    FilterOperator.NotEquals => (cellData != filterValue),

                    FilterOperator.StartsWith => cellData.StartsWith(filterValue),

                    FilterOperator.EndsWith => cellData.EndsWith(filterValue),

                    _ => true
                };
            }

            if (filterModel.Type == CellDataType.Number)
            {
                if (!int.TryParse(cellData, out var cellNumber) || !int.TryParse(filterValue, out var filterNumber))
                    return false;

                return filterOperator switch
                {
                    FilterOperator.Equals => (cellNumber == filterNumber),

                    FilterOperator.NotEquals => (cellNumber != filterNumber),

                    FilterOperator.GreaterThan => (cellNumber > filterNumber),

                    FilterOperator.LessThan => (cellNumber < filterNumber),

                    _ => true
                };
            }

            if (filterModel.Type == CellDataType.Boolean)
            {
                cellData = cellData.ToLower();

                filterValue = filterValue.ToLower();

                return cellData.Contains(filterValue);
            }

            if (filterModel.Type == CellDataType.Date)
            {
                if (!DateTime.TryParse(cellData, out _) || !DateTime.TryParse(filterValue, out _))
                    return false;

                var columnData = Convert.ToDateTime(cellData).ToString(gridColumn.DateFormat);
                var columnFilter = Convert.ToDateTime(filterValue).ToString(gridColumn.DateFormat);

                var result = columnData.CompareTo(columnFilter);

                return filterOperator switch
                {
                    FilterOperator.Equals => result == 0,

                    FilterOperator.NotEquals => result != 0,

                    FilterOperator.GreaterThan => result > 0,

                    FilterOperator.LessThan => result < 0,

                    _ => true
                };
            }

            return true;
        }

        private bool CheckFirstPart(TreeRowNode<TItem> tree, AdvancedFilterModel filterModel, List<GridColumn<TItem>> gridColumns, CellParams<TItem> cellParams)
        {
            if (!IsValidModelFirstPart(filterModel)) return true;

            return Check(tree, filterModel.Value, filterModel.Operator, filterModel, gridColumns, cellParams);
        }

        private bool CheckSecondPart(TreeRowNode<TItem> tree, AdvancedFilterModel filterModel, List<GridColumn<TItem>> gridColumns, CellParams<TItem> cellParams)
        {
            if (!IsValidModelSecondPart(filterModel)) return true;

            return Check(tree, filterModel.AdditionalValue, filterModel.AdditionalOperator, filterModel, gridColumns, cellParams);
        }

        private bool Show(TreeRowNode<TItem> tree, List<AdvancedFilterModel> advancedFilterList, List<GridColumn<TItem>> gridColumns, CellParams<TItem> cellParams)
        {
            if (!tree.Data.IsGroup)
            {
                tree.Data.AdvShow = true;
                foreach (var filterModel in advancedFilterList)
                {
                    var valueFirstPart = CheckFirstPart(tree, filterModel, gridColumns, cellParams);
                    var valueSecondPart = CheckSecondPart(tree, filterModel, gridColumns, cellParams);

                    if (filterModel.Condition == FilterCondition.None)
                        tree.Data.AdvShow = valueFirstPart;
                    if (filterModel.Condition == FilterCondition.And)
                        tree.Data.AdvShow = valueFirstPart && valueSecondPart;
                    if (filterModel.Condition == FilterCondition.Or)
                        tree.Data.AdvShow = valueFirstPart || valueSecondPart;

                    if (!tree.Data.AdvShow) break;
                }
            }
            else
            {
                tree.Data.AdvShow = false;
                foreach (var child in tree.Children)
                    if (Show(child, advancedFilterList, gridColumns, cellParams))
                        tree.Data.AdvShow = true;
            }
            return tree.Data.AdvShow;
        }
    }
}
