using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Enums;
using Dnet.Blazor.Components.Grid.Infrastructure.Models;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private async void AddGroup(string columnName)
    {
        if (!_rowNodes.Any() || _treeRn == null) return;

        _activeGroups++;
        _searchModel.PaginationModel.CurrentPage = 1;

        AuxAddGroup(columnName);

        if (GridOptions.GroupDefaultExpanded && _treeRn != null) 
            _treeRn = ExpandCollapseTreeRowNode(_treeRn, true);

        FilterBy();
        AdvancedFilterBy();

        await Update();
    }

    private void AuxAddGroup(string dataField)
    {
        if (_groupByColumns.Contains(dataField)) return;

        _groupByColumns.Add(dataField);

        if (GridOptions.EnableServerSideGrouping)
        {
            OnGroupingChanged.InvokeAsync(new GroupModel()
            {
                ColumnName = dataField,
                Operation = GroupOperation.Add
            });
        }
        else
        {
            var cellParams = new CellParams<TItem>
            {
                GridApi = _gridApi,
            };

            var lastId = _nextId;
            var gridColumn = _gridColumns.Find(e => e.DataField == dataField);

            _treeRn = GroupingService.AddGroupByColumn(_treeRn, gridColumn, _gridColumns, ref lastId, cellParams);
            _nextId = lastId;
        }
    }

    private async void DeleteGroup(string columnName)
    {
        if (!_rowNodes.Any() || _treeRn == null) return;

        _activeGroups--;
        _searchModel.PaginationModel.CurrentPage = 1;

        AuxDeleteGroup(columnName);

        FilterBy();
        AdvancedFilterBy();

        await Update();
    }

    private void AuxDeleteGroup(string dataField)
    {
        _groupByColumns.Remove(dataField);

        if (GridOptions.EnableServerSideGrouping)
        {
            OnGroupingChanged.InvokeAsync(new GroupModel()
            {
                ColumnName = dataField,
                Operation = GroupOperation.Delete
            });
        }
        else
        {
            var lastId = _nextId;

            var cellParams = new CellParams<TItem>
            {
                GridApi = _gridApi,
            };

            var gridColumn = _gridColumns.Find(e => e.DataField == dataField);

            _treeRn = GroupingService.RemoveGroupByColumn(_treeRn, gridColumn, _gridColumns, ref lastId, cellParams);
            _nextId = lastId;
        }
    }

    public async Task ExpandCollapse(bool isExpanded)
    {
        if (isExpanded)
        {
            await ExpandAll();
        }
        else
        {
            await CollapseAll();
        }
    }

    public async Task ExpandAll()
    {
        _treeRn = ExpandCollapseTreeRowNode(_treeRn, true);
        _isExpanded = true;

        if (_blgHeader != null) _blgHeader.ActiveRender();

        await Update();
    }

    public async Task CollapseAll()
    {
        _treeRn = ExpandCollapseTreeRowNode(_treeRn, false);
        _treeRn.Data.Expanded = true;
        _isExpanded = false;

        if (_blgHeader != null) _blgHeader.ActiveRender();

        _searchModel.PaginationModel.CurrentPage = 1;

        await Update();
    }

    private async void ChangeExpanded(long id)
    {
        var index = _rowNodes.FindIndex(rowNode => rowNode.RowNodeId == id);
        _rowNodes[index].Expanded = !_rowNodes[index].Expanded;
        await Update();
    }
}