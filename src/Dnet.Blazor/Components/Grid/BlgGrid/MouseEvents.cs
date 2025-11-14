using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private void MouseOverLeft(long rowNodeId)
    {
        if (_updatingGridData) return;

        var rowNode = _rowNodes.Where(p => p.RowNodeId == rowNodeId).Select(p => p).FirstOrDefault();

        if (_previousHoverRowNode != null && rowNodeId == -1)
        {
            _previousHoverRowNode.HoverThisNode(false);
        }

        if (rowNodeId != -1)
        {
            rowNode?.HoverThisNode(true);
            _previousHoverRowNode = rowNode;
        }

        _blgCenter?.ActiveRender();
        if (_pinnedRight) _blgPinnedRight?.ActiveRender();

        StateHasChanged();
    }

    private void MouseOverCenter(long rowNodeId)
    {
        if (_updatingGridData) return;

        var rowNode = _rowNodes.Where(p => p.RowNodeId == rowNodeId).Select(p => p).FirstOrDefault();

        if (_previousHoverRowNode != null && rowNodeId == -1)
        {
            _previousHoverRowNode.HoverThisNode(false);
        }

        if (rowNodeId != -1)
        {
            rowNode?.HoverThisNode(true);
            _previousHoverRowNode = rowNode;
        }

        if (_pinnedRight) _blgPinnedRight?.ActiveRender();
        if (_pinnedLeft) _blgPinnedLeft?.ActiveRender();

        StateHasChanged();
    }

    private void MouseOverRight(long rowNodeId)
    {
        if (_updatingGridData) return;

        var rowNode = _rowNodes.Where(p => p.RowNodeId == rowNodeId).Select(p => p).FirstOrDefault();

        if (_previousHoverRowNode != null && rowNodeId == -1)
        {
            _previousHoverRowNode.HoverThisNode(false);
        }

        if (rowNodeId != -1)
        {
            rowNode?.HoverThisNode(true);
            _previousHoverRowNode = rowNode;
        }

        _blgCenter?.ActiveRender();
        if (_pinnedLeft) _blgPinnedLeft?.ActiveRender();

        StateHasChanged();
    }

    private void MouseMove(MouseEventArgs e)
    {
        if (!_mouseDown) return;

        var clientX = e.ClientX;

        if (_columnSelected != null && _columnSelected.Width + (int)clientX - _lastColumnPosition > GridOptions.ColumnMinWidth)
        {
            _columnSelected.Width += (int)clientX - _lastColumnPosition;
            eBodyHorizontalScrollContainerWidth = eBodyHorizontalScrollContainerWidth + (int)clientX - _lastColumnPosition;
            _lastColumnPosition = (int)clientX;

            if (_blgHeader != null) _blgHeader.ActiveRender();
        }
    }

    private void MouseUp()
    {
        if (!_mouseDown) return;

        _mouseDown = false;
        _lastColumnPosition = 0;
    }

    private void OnMouseDown(Tuple<string, int> redimensionInfo)
    {
        _mouseDown = true;
        _columnSelected = _gridColumns.FirstOrDefault(p => p.DataField == redimensionInfo.Item1);
        _lastColumnPosition = redimensionInfo.Item2;
    }

    [JSInvokable]
    public void MouseLeave()
    {
        if (_mouseDown)
        {
            _mouseDown = false;
            _lastColumnPosition = 0;
        }
    }
}