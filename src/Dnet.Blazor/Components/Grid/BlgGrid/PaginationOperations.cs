namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private async void PaginationChanged(int currentPage)
    {
        _searchModel.PaginationModel.CurrentPage = currentPage;

        if (GridOptions.EnableServerSidePagination)
        {
            await OnPaginationChanged.InvokeAsync(_searchModel);
        }
        else
            await AssignRangeRenderedRows();

        _blgCenter?.ActiveRender();
        if (_pinnedLeft) _blgPinnedLeft?.ActiveRender();
        if (_pinnedRight) _blgPinnedRight?.ActiveRender();
    }

    private void GoToFirstPage(int currentPage)
    {
        PaginationChanged(currentPage);
    }

    private void GoToPreviousPage(int currentPage)
    {
        PaginationChanged(currentPage);
    }

    private void GoToNextPage(int currentPage)
    {
        PaginationChanged(currentPage);
    }

    private void GoToLastPage(int currentPage)
    {
        PaginationChanged(currentPage);
    }

    private void GoToSpecificPage(int specificPage)
    {
        PaginationChanged(specificPage);
    }

    private async Task AssignRangeRenderedRows()
    {
        if (GridOptions.Pagination)
        {
            var currentPage = _searchModel.PaginationModel.CurrentPage;
            var pageSize = _searchModel.PaginationModel.PageSize;
            var itemsCount = _searchModel.PaginationModel.ItemsCount;

            if (currentPage <= 0) currentPage = 1;

            var (startIndex, count) = PaginatorService.GetRangePage(currentPage, pageSize, itemsCount);

            _renderedRowNodes = _rowNodes.GetRange(startIndex, count);
        }
        else
        {
            _renderedRowNodes = _rowNodes;
        }

        ManageGridCellSpanning();

        _gridApi.RenderedRowNodes = _renderedRowNodes;

        if (GridOptions.UseVirtualization)
        {
            if (!_firstRender) await DefaultVirtualization();
        }
        else
        {
            _itemsToShow = _renderedRowNodes;
            StateHasChanged();
        }
    }
}