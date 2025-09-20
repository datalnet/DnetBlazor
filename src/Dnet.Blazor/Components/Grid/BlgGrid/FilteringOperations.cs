using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private void FilterBy()
    {
        if (GridOptions.EnableServerSideFilter)
        {
            AdvancedFilterBy();
        }
        else
        {
            var filterList = new List<FilterModel>();

            foreach (var gridColumn in _gridColumns)
                if (gridColumn.CellDataType != CellDataType.None && !string.IsNullOrEmpty(gridColumn.Filter))
                {
                    filterList.Add(new FilterModel
                    {
                        Filter = gridColumn.Filter,
                        DataDield = gridColumn.DataField,
                        Type = gridColumn.CellDataType
                    });
                }

            var cellParams = new CellParams<TItem>
            {
                GridApi = _gridApi,
            };

            _treeRn = FilteringService.FilterBy(_treeRn, filterList, _gridColumns, cellParams);
            OnFilterChanged.InvokeAsync(_searchModel);
        }
    }

    private void AdvancedFilterBy()
    {
        _searchModel.AdvancedFilterModels = new List<AdvancedFilterModel>();

        if (GridOptions.EnableServerSideFilter)
        {
            var filterModels = new List<AdvancedFilterModel>();

            foreach (var gridColumn in _gridColumns)
                if (gridColumn.CellDataType != CellDataType.None && !string.IsNullOrEmpty(gridColumn.Filter))
                {
                    var filterOperator = gridColumn.CellDataType switch
                    {
                        CellDataType.Number => FilterOperator.Equals,
                        CellDataType.Boolean => FilterOperator.Equals,
                        _ => FilterOperator.Contains
                    };

                    var advancedFilterModel = new AdvancedFilterModel
                    {
                        AdditionalOperator = FilterOperator.None,
                        AdditionalValue = null,
                        Column = gridColumn.DataField,
                        Condition = FilterCondition.None,
                        Operator = filterOperator,
                        Type = gridColumn.CellDataType,
                        Value = gridColumn.Filter
                    };

                    filterModels.Add(advancedFilterModel);
                }

            _searchModel.AdvancedFilterModels.AddRange(filterModels);
        }

        var advancedFilterList = _gridColumns.Where(e => e.CellDataType != CellDataType.None && !string.IsNullOrEmpty(e.AdvancedFilterModel.Value))
            .Select(e => e.AdvancedFilterModel).ToList();

        var advancedFilterModels = advancedFilterList.Count > 0 ? advancedFilterList : new List<AdvancedFilterModel>();

        _searchModel.AdvancedFilterModels.AddRange(advancedFilterModels);

        if (GridOptions.EnableServerSideAdvancedFilter)
        {
            OnAdvancedFilterChanged.InvokeAsync(_searchModel);
        }
        else
        {
            var cellParams = new CellParams<TItem>
            {
                GridApi = _gridApi,
            };

            _treeRn = AdvancedFilteringService.FilterBy(_treeRn, advancedFilterList, _gridColumns, cellParams);
            OnAdvancedFilterChanged.InvokeAsync(_searchModel);
        }
    }

    private async void OnAdvancedFilter()
    {
        _searchModel.PaginationModel.CurrentPage = 1;
        AdvancedFilterBy();
        await Update();
    }

    private async void OnFilter()
    {
        _searchModel.PaginationModel.CurrentPage = 1;
        FilterBy();
        await Update();
    }

    public List<AdvancedFilterModel> ConverToAdvancedFilter(List<FilterModel> filterModels)
    {
        var advancedFilterModels = new List<AdvancedFilterModel>();

        foreach (var filterModel in filterModels)
        {
            var filterOperator = filterModel.Type switch
            {
                CellDataType.Number => FilterOperator.Equals,
                CellDataType.Boolean => FilterOperator.Equals,
                _ => FilterOperator.Contains
            };

            var advancedFilterModel = new AdvancedFilterModel
            {
                AdditionalOperator = FilterOperator.None,
                AdditionalValue = null,
                Column = filterModel.DataDield,
                Condition = FilterCondition.None,
                Operator = filterOperator,
                Type = filterModel.Type,
                Value = filterModel.Filter
            };

            advancedFilterModels.Add(advancedFilterModel);
        }

        return advancedFilterModels;
    }

    public async void DeselectAllFiltered()
    {
        foreach (var gridcolumn in _gridColumns.Where(gridcolumn => gridcolumn.CellDataType != CellDataType.None))
        {
            gridcolumn.Filter = "";
            var filterModel = gridcolumn.AdvancedFilterModel;
            filterModel.AdditionalValue = filterModel.Value = "";
            filterModel.AdditionalOperator = FilterOperator.None;
            filterModel.Operator = GridOptions.DefaultAdvancedFilterOperator;
            filterModel.Condition = FilterCondition.None;
            filterModel.FilterLinkCondition = FilterCondition.And;
        }

        _treeRn = BuildRowNodes();
        await Update();

        if (_blgHeader != null) _blgHeader.ActiveRender();
    }
}