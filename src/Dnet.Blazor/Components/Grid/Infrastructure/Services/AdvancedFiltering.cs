using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Infrastructure.Models.SearchModels;
using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;
using System;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class AdvancedFiltering<TItem> : IAdvancedFiltering<TItem>
    {
        public List<GridColumn<TItem>> InitAdvancedFilterModels(List<GridColumn<TItem>> gridColumns, FilterOperator defaultAdvancedFilterOperator)
        {
            foreach (var gridColumn in gridColumns)
            {
                if (gridColumn.AdvancedFilterModel.Column == null && gridColumn.CellDataType != CellDataType.None && gridColumn.EnableAdvancedFilter)
                {
                    gridColumn.AdvancedFilterModel = new AdvancedFilterModel
                    {
                        Operator = gridColumn.CellDataType == CellDataType.Text ? defaultAdvancedFilterOperator : FilterOperator.None,
                        AdditionalOperator = FilterOperator.None,
                        Type = gridColumn.CellDataType,
                        Column = gridColumn.DataField,
                        Value = "",
                        AdditionalValue = ""
                    };
                }
            }
            return gridColumns;
        }

        public TreeRowNode<TItem> FilterBy(TreeRowNode<TItem> tree, List<AdvancedFilterModel> advancedFilterList, List<GridColumn<TItem>> gridColumns, CellParams<TItem> cellParams)
        {
            // Preprocess to avoid repetitive Find operation inside Check
            var columnDictionary = gridColumns.ToDictionary(col => col.DataField, col => col);

            foreach (var child in tree.Children)
                Show(child, advancedFilterList, columnDictionary, cellParams);
            return tree;
        }

        private bool Check(TreeRowNode<TItem> tree, string filterValue, FilterOperator filterOperator, AdvancedFilterModel filterModel, Dictionary<string, GridColumn<TItem>> columnDictionary, CellParams<TItem> cellParams)
        {
            if (!columnDictionary.TryGetValue(filterModel.Column, out var gridColumn))
                return true;

            cellParams.RowData = tree.Data.RowData;
            cellParams.GridColumn = gridColumn;
            cellParams.RowNode = tree.Data;

            var cellData = gridColumn.CellDataFn(cellParams)?.ToString();
            
            if (cellData == null) return false;

            // Switch based on type to avoid repetitive type checks
            switch (filterModel.Type)
            {
                case CellDataType.Text:
                    return ApplyTextFilter(cellData, filterValue, filterOperator);
                case CellDataType.Number:
                    return ApplyNumberFilter(cellData, filterValue, filterOperator);
                case CellDataType.Boolean:
                    return ApplyBooleanFilter(cellData, filterValue);
                case CellDataType.Date:
                    return ApplyDateFilter(cellData, filterValue, filterOperator, gridColumn.DateFormat);
                default:
                    return true;
            }
        }

        private bool ApplyTextFilter(string cellData, string filterValue, FilterOperator filterOperator)
        {
            cellData = cellData.ToLower();

            filterValue = filterValue.ToLower();

            return filterOperator switch
            {
                FilterOperator.Contains => cellData.Contains(filterValue),
                FilterOperator.Equals => cellData == filterValue,
                FilterOperator.NotContains => !cellData.Contains(filterValue),
                FilterOperator.NotEquals => cellData != filterValue,
                FilterOperator.StartsWith => cellData.StartsWith(filterValue),
                FilterOperator.EndsWith => cellData.EndsWith(filterValue),
                _ => true,
            };
        }

        private bool ApplyNumberFilter(string cellData, string filterValue, FilterOperator filterOperator)
        {
            if (!int.TryParse(cellData, out var cellNumber) || !int.TryParse(filterValue, out var filterNumber))
                return false;

            return filterOperator switch
            {
                FilterOperator.Equals => cellNumber == filterNumber,
                FilterOperator.NotEquals => cellNumber != filterNumber,
                FilterOperator.GreaterThan => cellNumber > filterNumber,
                FilterOperator.LessThan => cellNumber < filterNumber,
                _ => true,
            };
        }

        private bool ApplyBooleanFilter(string cellData, string filterValue)
        {
            return cellData.ToLower().Contains(filterValue.ToLower());
        }

        private bool ApplyDateFilter(string cellData, string filterValue, FilterOperator filterOperator, string dateFormat)
        {
            if (!DateTime.TryParse(cellData, out var cellDate) || !DateTime.TryParse(filterValue, out var filterDate))
                return false;

            int result = DateTime.Compare(cellDate, filterDate);

            return filterOperator switch
            {
                FilterOperator.Equals => result == 0,
                FilterOperator.NotEquals => result != 0,
                FilterOperator.GreaterThan => result > 0,
                FilterOperator.LessThan => result < 0,
                _ => true,
            };
        }

        private bool Show(TreeRowNode<TItem> tree, List<AdvancedFilterModel> advancedFilterList, Dictionary<string, GridColumn<TItem>> columnDictionary, CellParams<TItem> cellParams)
        {
            if (!tree.Data.IsGroup)
            {
                tree.Data.AdvShow = advancedFilterList.All(filterModel =>
                {
                    bool firstPartResult = CheckPart(tree, filterModel, columnDictionary, cellParams, true);

                    bool secondPartResult = CheckPart(tree, filterModel, columnDictionary, cellParams, false);

                    switch (filterModel.Condition)
                    {
                        case FilterCondition.None:
                            return firstPartResult;
                        case FilterCondition.And:
                            return firstPartResult && secondPartResult;
                        case FilterCondition.Or:
                            return firstPartResult || secondPartResult;
                        default:
                            return true;
                    };
                });
            }
            else
            {
                tree.Data.AdvShow = tree.Children.Any(child => Show(child, advancedFilterList, columnDictionary, cellParams));
            }

            return tree.Data.AdvShow;
        }

        private bool CheckPart(TreeRowNode<TItem> tree, AdvancedFilterModel filterModel, Dictionary<string, GridColumn<TItem>> columnDictionary, CellParams<TItem> cellParams, bool isFirstPart)
        {
            var partValue = isFirstPart ? filterModel.Value : filterModel.AdditionalValue;

            var partOperator = isFirstPart ? filterModel.Operator : filterModel.AdditionalOperator;

            return Check(tree, partValue, partOperator, filterModel, columnDictionary, cellParams);
        }
    }
}