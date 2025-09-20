using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private async void OnSortBy(string dataField)
    {
        if (!_rowNodes.Any() || _treeRn == null) return;

        var gridColumn = _gridColumns.Find(e => e.DataField == dataField);

        SortingService.UpdateOrder(_gridColumns, gridColumn);

        _searchModel.PaginationModel.CurrentPage = 1;

        SortBy(gridColumn);

        await Update();
    }

    private async void SortBy(GridColumn<TItem> gridColumn)
    {
        var cellParams = new CellParams<TItem>
        {
            GridApi = _gridApi,
        };

        if (gridColumn.SortStatus == SortOrder.None && !GridOptions.EnableServerSideSorting)
        {
            InitializeGridWorkingData();
            await InitializeGrid();
            return;
        }

        _searchModel.SortModel = new SortModel
        {
            ColumnName = gridColumn.DataField,
            Order = gridColumn.SortStatus
        };

        if (GridOptions.EnableServerSideSorting)
        {
            await OnSortingChanged.InvokeAsync(_searchModel);
        }
        else
            _treeRn = _groupByColumns.Contains(gridColumn.DataField)
                ? SortingService.SortGroupingBy(_treeRn, gridColumn, cellParams)
                : SortingService.SortBy(_treeRn, gridColumn, cellParams);
    }
}