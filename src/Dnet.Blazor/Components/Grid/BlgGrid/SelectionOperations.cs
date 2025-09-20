using Dnet.Blazor.Components.Grid.Infrastructure.Entities;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private async void CellClicked(CellClikedEventData cellClikedEventData)
    {
        _blgCenter?.ActiveRender();

        if (_pinnedLeft) _blgPinnedLeft?.ActiveRender();
        if (_pinnedRight) _blgPinnedRight?.ActiveRender();

        var gridColumn = _gridColumns.Find(e => e.ColumnId == cellClikedEventData.CellId);
        var rownode = _rowNodes.Find(e => e.RowNodeId == cellClikedEventData.RowNodeId);

        var cellClikedData = new CellClikedData<TItem>();
        cellClikedData.ColumnId = gridColumn.ColumnId;
        cellClikedData.ColumnOrder = gridColumn.ColumnOrder;
        cellClikedData.HeaderName = gridColumn.HeaderName;
        cellClikedData.DataField = gridColumn.DataField;
        cellClikedData.AdvancedFilterModel = gridColumn.AdvancedFilterModel;
        cellClikedData.RowNode = rownode;

        await OnCellClicked.InvokeAsync(cellClikedData);
    }

    private async void RowClicked(long rowNodeId)
    {
        var rowNode = _rowNodes.Find(e => e.RowNodeId == rowNodeId);

        _blgCenter?.ActiveRender();
        if (_pinnedLeft) _blgPinnedLeft?.ActiveRender();
        if (_pinnedRight) _blgPinnedRight?.ActiveRender();

        await OnRowClicked.InvokeAsync(rowNode);
    }

    private async void RowDoubleClicked(long rowNodeId)
    {
        var rowNode = _rowNodes.Find(e => e.RowNodeId == rowNodeId);
        await OnRowDoubleClicked.InvokeAsync(rowNode);
    }

    private async void SelectionChanged(List<long> rowNodeIds)
    {
        _blgCenter?.ActiveRender();
        if (_pinnedLeft) _blgPinnedLeft?.ActiveRender();
        if (_pinnedRight) _blgPinnedRight?.ActiveRender();

        var rowNodes = _rowNodes.Where(p => p.RowData is not null && rowNodeIds.Contains(p.RowNodeId)).Select(p => p).ToList();

        await OnSelectionChanged.InvokeAsync(rowNodes);
    }

    private async void ChangeSelectAllNodes(bool value)
    {
        if (!_rowNodes.Any() || _treeRn == null) return;

        AuxChangeSelectAllNodes(_treeRn, value);

        var selectedRowNodes = new List<RowNode<TItem>>();

        if (value)
        {
            var flattenTree = FlattenTree(_treeRn, 0);
            selectedRowNodes = _rowNodes = flattenTree.Where(e => (e.IsSelected() && e.RowData != null && !e.IsGroup) || (e.IsSelected() && e.IsGroup)).Select(p => p).ToList();
        }
        else
        {
            _rowNodes = FlattenTree(_treeRn, 0).Where(e => (!e.IsSelected() && e.RowData != null && !e.IsGroup) || (!e.IsSelected() && e.IsGroup)).Select(p => p).ToList();
        }

        _blgCenter?.ActiveRender();
        if (_pinnedRight) _blgPinnedRight?.ActiveRender();
        if (_pinnedRight) _blgPinnedRight?.ActiveRender();

        var rowNodes = selectedRowNodes.Where(p => p.RowData is not null).ToList();

        if (_blgCenter is not null) _blgCenter.SelectallNotify(value);

        await OnSelectionChanged.InvokeAsync(rowNodes);
    }

    public async Task SelectAll()
    {
        _treeRn = SelectTreeRowNode(_treeRn, true);
        await Update();
    }

    public async Task DeselectAll()
    {
        _treeRn = SelectTreeRowNode(_treeRn, false);
        await Update();
    }

    public List<RowNode<TItem>> GetSelectedNodes()
    {
        return _rowNodes.Where(e => e.IsSelected()).ToList();
    }
}