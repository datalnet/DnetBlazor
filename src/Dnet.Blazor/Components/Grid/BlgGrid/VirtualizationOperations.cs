using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Virtualize;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private async Task DefaultVirtualization()
    {
        _lastRenderedItemCount = 0;

        CalculateItemDistribution(0, 0, _containerSize, out var itemsBefore, out var visibleItemCapacity);

        UpdateItemDistribution(itemsBefore, visibleItemCapacity, true);

        await UpdateGridData();
    }

    private async Task UpdateGridData()
    {
        var lastItemIndex = Math.Min(_itemsBefore + _visibleItemCapacity, _itemCount);

        _itemsToShow = _loadedItems.Skip(_itemsBefore - _loadedItemsStartIndex)
                                   .Take(lastItemIndex - _loadedItemsStartIndex)
                                   .ToList();

        foreach (var rowNode in _itemsToShow)
            rowNode.First = false;

        if (_itemsToShow.Count > 0)
            _itemsToShow[0].First = true;

        _blgCenter?.ActiveRender();

        if (_pinnedLeft) _blgPinnedLeft?.ActiveRender();

        if (_pinnedRight) _blgPinnedRight?.ActiveRender();

        await InvokeAsync(StateHasChanged);

        _updatingGridData = false;
    }

    private string GetSpacerStyle(int itemsInSpacer) => $"height: {itemsInSpacer * _itemSize}px; width: {eBodyHorizontalScrollContainerWidth}px";

    async Task IVirtualizeJsCallbacks.OnBeforeSpacerVisible(float spacerSize, float spacerSeparation, float containerSize)
    {
        if (_previousHoverRowNode != null) _previousHoverRowNode.HoverThisNode(false);

        _updatingGridData = true;

        _containerSize = containerSize;

        CalculateItemDistribution(spacerSize, spacerSeparation, containerSize, out var itemsBefore, out var visibleItemCapacity);

        // Since we know the before spacer is now visible, we absolutely have to slide the window up
        // by at least one element. If we're not doing that, the previous item size info we had must
        // have been wrong, so just move along by one in that case to trigger an update and apply the
        // new size info.
        if (itemsBefore == _itemsBefore && itemsBefore > 0)
        {
            itemsBefore--;
        }

        UpdateItemDistribution(itemsBefore, visibleItemCapacity);

        await UpdateGridData();
    }

    async Task IVirtualizeJsCallbacks.OnAfterSpacerVisible(float spacerSize, float spacerSeparation, float containerSize)
    {
        if (_previousHoverRowNode != null) _previousHoverRowNode.HoverThisNode(false);

        _updatingGridData = true;

        _containerSize = containerSize;

        CalculateItemDistribution(spacerSize, spacerSeparation, containerSize, out var itemsAfter, out var visibleItemCapacity);

        var itemsBefore = Math.Max(0, _itemCount - itemsAfter - visibleItemCapacity);

        // Since we know the after spacer is now visible, we absolutely have to slide the window down
        // by at least one element. If we're not doing that, the previous item size info we had must
        // have been wrong, so just move along by one in that case to trigger an update and apply the
        // new size info.
        if (itemsBefore == _itemsBefore && itemsBefore > 0)
        {
            itemsBefore--;
        }

        UpdateItemDistribution(itemsBefore, visibleItemCapacity);

        await UpdateGridData();
    }

    private void CalculateItemDistribution(
        float spacerSize,
        float spacerSeparation,
        float containerSize,
        out int itemsInSpacer,
        out int visibleItemCapacity)
    {
        if (_lastRenderedItemCount > 0)
        {
            _itemSize = (spacerSeparation - (_lastRenderedPlaceholderCount * _itemSize)) / _lastRenderedItemCount;
        }

        if (_itemSize <= 0)
        {
            // At this point, something unusual has occurred, likely due to misuse of this component.
            // Reset the calculated item size to the user-provided item size.
            _itemSize = GridOptions.RowHeight;
        }

        itemsInSpacer = Math.Max(0, (int)Math.Floor(spacerSize / _itemSize) - 1 - OverscanCount);

        visibleItemCapacity = (int)Math.Ceiling(containerSize / _itemSize) + 2 * OverscanCount;
    }

    private void UpdateItemDistribution(int itemsBefore, int visibleItemCapacity, bool forceRefresh = false)
    {
        if (itemsBefore + visibleItemCapacity > _itemCount)
        {
            itemsBefore = Math.Max(0, _itemCount - visibleItemCapacity);
        }

        if (itemsBefore == _itemsBefore && visibleItemCapacity == _visibleItemCapacity && forceRefresh == false) return;

        _itemsBefore = itemsBefore;

        _visibleItemCapacity = visibleItemCapacity;

        var refreshTask = RefreshDataAsync();

        if (!refreshTask.IsCompleted)
        {
            StateHasChanged();
        }
    }

    private async Task RefreshDataAsync()
    {
        _refreshCts?.Cancel();

        _refreshCts = new CancellationTokenSource();

        var cancellationToken = _refreshCts.Token;

        var request = new ItemsProviderRequest(_itemsBefore, _visibleItemCapacity, cancellationToken);

        try
        {
            var result = await _itemsProvider(request);

            // Only apply result if the task was not canceled.
            if (!cancellationToken.IsCancellationRequested)
            {
                _itemCount = result.TotalItemCount;
                _loadedItems = result.Items;
                _loadedItemsStartIndex = request.StartIndex;
            }
        }
        catch (Exception e)
        {
            if (e is OperationCanceledException oce && oce.CancellationToken == cancellationToken)
            {
                // No-op; we canceled the operation, so it's fine to suppress this exception.
            }
            else
            {
                // Cache this exception so the renderer can throw it.
                _refreshException = e;
                StateHasChanged();
            }
        }
    }

    private ValueTask<ItemsProviderResult<RowNode<TItem>>> DefaultItemsProvider(ItemsProviderRequest request)
    {
        return ValueTask.FromResult(new ItemsProviderResult<RowNode<TItem>>(_renderedRowNodes!.Skip(request.StartIndex).Take(request.Count), _renderedRowNodes!.Count));
    }

    [JSInvokable]
    public async Task OnTouchMove(ScrollInfo scrollInfo)
    {
        var result = await BlGridInterop.GetElementScrollLeft(_eBodyHorizontalScrollViewport);

        _transformX = $"-{Math.Floor(result)}px";

        StateHasChanged();
    }
}