using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private void InitializeGridWorkingData()
    {
        if (!string.IsNullOrEmpty(GridOptions.GridClass)) 
            _gridClasses += " " + GridOptions.GridClass;

        _mouseDown = false;
        _lastColumnPosition = 0;

        _searchModel = new SearchModel
        {
            PaginationModel = new PaginationModel
            {
                CurrentPage = GridOptions.PaginationStartPage,
                PageSize = GridOptions.PaginationPageSize
            }
        };

        _treeRn = BuildRowNodes();
        _rowNodes = FlattenTree(_treeRn, 0).FindAll(e => e.Show).ToList();
        _gridApi.RowNodes = _rowNodes;
        _gridApi.TreeRowNodes = _treeRn;
    }

    private TreeRowNode<TItem> InitializeTree()
    {
        var treeRn = new TreeRowNode<TItem>
        {
            Data = new RowNode<TItem>
            {
                IsGroup = true,
                Show = false,
                Expanded = true
            },
            Children = new List<TreeRowNode<TItem>>()
        };

        return treeRn;
    }

    private async Task InitializeGrid()
    {
        var numberOfRows = !GridOptions.EnableServerSidePagination ? _rowNodes.Count : GridOptions.NumberOfRows;
        _searchModel.PaginationModel.ItemsCount = numberOfRows;

        // In debug mode do not do Default operations
        if (!GridOptions.IsDebugMode) 
            OperationsByDefault();

        if (GridOptions.GroupDefaultExpanded && _treeRn != null) 
            _treeRn = ExpandCollapseTreeRowNode(_treeRn, true);

        await Update();
    }

    private void OperationsByDefault()
    {
        if (GridOptions.GroupDefaultExpanded) 
            _isExpanded = true;

        if (!_rowNodes.Any()) 
            return;

        if (!GridOptions.EnableServerSideAdvancedFilter) 
            AdvancedFilterByDefault();

        if (!GridOptions.EnableServerSideFilter) 
            FilterByDefault();

        if (!GridOptions.EnableServerSideSorting) 
            SortByDefault();

        if (!GridOptions.EnableServerSideGrouping) 
            GroupByDefault();
    }

    private void AdvancedFilterByDefault()
    {
        AdvancedFilterBy();
    }

    private void FilterByDefault()
    {
        FilterBy();
    }

    private void SortByDefault()
    {
        var index = _gridColumns.FindIndex(e => e.Sortable && e.SortStatus != SortOrder.None);

        if (index == -1) return;

        SortBy(_gridColumns[index]);
    }

    private void GroupByDefault()
    {
        if (!GridOptions.EnableGrouping) return;

        _groupByColumns.Clear();

        for (var i = 0; i < _gridColumns.Count; ++i)
        {
            var index = _gridColumns.FindIndex(e => e.RowGroup && e.RowGroupIndex == i);

            if (index == -1) break;

            _activeGroups++;
            AuxAddGroup(_gridColumns[index].DataField);
        }
    }
}