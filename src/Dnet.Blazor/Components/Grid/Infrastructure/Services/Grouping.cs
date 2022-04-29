using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class Grouping<TItem> : IGrouping<TItem>
    {
        public TreeRowNode<TItem> AddGroupByColumn(TreeRowNode<TItem> tree,
            GridColumn<TItem> gridColumn,
            List<GridColumn<TItem>> gridColumns,
            ref long lastId,
            CellParams<TItem> cellParams)
        {
            if (gridColumn.RowGroup && gridColumn.RowGroupIndex != -1)
                return AuxAddGroupByColumn(tree, gridColumn, ref lastId, cellParams);

            var groupedGridColumns = gridColumns.FindAll(e => e.RowGroup);

            var lastLevel = -1;

            if (groupedGridColumns.Count > 0) lastLevel = groupedGridColumns.Max(e => e.RowGroupIndex);

            gridColumn.RowGroup = true;

            gridColumn.RowGroupIndex = lastLevel + 1;

            return AuxAddGroupByColumn(tree, gridColumn, ref lastId, cellParams);
        }

        private long GetId(ref long _nextId)
        {
            _nextId++;
            return _nextId;
        }

        public TreeRowNode<TItem> AuxAddGroupByColumn(TreeRowNode<TItem> tree,
            GridColumn<TItem> gridColumn,
            ref long lastId,
            CellParams<TItem> cellParams)
        {
            cellParams.GridColumn = gridColumn;
            cellParams.RowNode = tree.Data;

            if (tree.Data.IsGroup && !tree.Children[0].Data.IsGroup)
            {
                var groups = new List<List<TreeRowNode<TItem>>>();

                var dic = new Dictionary<string, int>();

                var nullGroup = new List<TreeRowNode<TItem>>();

                foreach (var child in tree.Children)
                {
                    cellParams.RowData = child.Data.RowData;

                    if (gridColumn.CellDataFn(cellParams) is null)
                    {
                        nullGroup.Add(child);
                    }
                    else
                    {
                        if (!dic.ContainsKey(gridColumn.CellDataFn(cellParams).ToString()))
                        {
                            dic[gridColumn.CellDataFn(cellParams).ToString()] = groups.Count;
                            groups.Add(new List<TreeRowNode<TItem>>() {child});
                        }
                        else
                        {
                            groups[dic[gridColumn.CellDataFn(cellParams).ToString()]].Add(child);
                        }
                    }
                }

                tree.Children = new List<TreeRowNode<TItem>>();

                if (nullGroup.Count > 0)
                    groups.Add(nullGroup);

                foreach (var group in groups)
                {
                    cellParams.RowData = group[0].Data.RowData;

                    tree.Children.Add(new TreeRowNode<TItem>()
                    {
                        Data = new RowNode<TItem>()
                        {
                            IsGroup = true,
                            Show = true,
                            AdvShow = true,
                            RowNodeId = GetId(ref lastId),
                        },
                        Children = group,
                        ColumnName = gridColumn.DataField,
                        Value = gridColumn.CellDataFn(cellParams) is null
                            ? null
                            : gridColumn.CellDataFn(cellParams).ToString()
                    });
                }
            }
            else
            {
                foreach (var child in tree.Children)
                    AuxAddGroupByColumn(child, gridColumn, ref lastId, cellParams);
            }

            return tree;
        }

        public TreeRowNode<TItem> RemoveGroupByColumn(TreeRowNode<TItem> tree,
            GridColumn<TItem> gridColumn,
            List<GridColumn<TItem>> gridColumns,
            ref long lastId,
            CellParams<TItem> cellParams)
        {
            var rowgroupindex = gridColumn.RowGroupIndex;

            gridColumn.RowGroup = false;

            gridColumn.RowGroupIndex = -1;

            foreach (var gridc in gridColumns)
                if (gridc.RowGroup && gridc.RowGroupIndex > rowgroupindex)
                    gridc.RowGroupIndex--;

            return AuxRemoveGroupByColumn(tree, gridColumn, gridColumns, ref lastId, cellParams);
        }

        public TreeRowNode<TItem> AuxRemoveGroupByColumn(TreeRowNode<TItem> tree,
                                                         GridColumn<TItem> gridColumn,
                                                         List<GridColumn<TItem>> gridColumns,
                                                         ref long lastId,
                                                         CellParams<TItem> cellParams)
        {
            cellParams.RowNode = tree.Data;

            if (tree.Children.Count > 0 && tree.Children[0].ColumnName == gridColumn.DataField)
            {
                if (!tree.Children[0].Children[0].Data.IsGroup || tree.Children[0].Children[0].Children[0].Data.IsGroup)
                {
                    var treeRowNodeList = new List<TreeRowNode<TItem>>();

                    foreach (var child in tree.Children)
                    foreach (var treeChildren in child.Children)
                        treeRowNodeList.Add(treeChildren);

                    tree.Children = treeRowNodeList;
                }
                else
                {
                    var groups = new List<List<TreeRowNode<TItem>>>();
                    var dic = new Dictionary<string, int>();
                    var subtreeGridColumn =
                        gridColumns.Find(e => e.DataField == tree.Children[0].Children[0].ColumnName);

                    cellParams.GridColumn = subtreeGridColumn;

                    var nullGroup = new List<TreeRowNode<TItem>>();

                    foreach (var treeChildren in tree.Children)
                    foreach (var subtreeChildren in treeChildren.Children)
                    foreach (var child in subtreeChildren.Children)
                    {
                        cellParams.RowData = child.Data.RowData;

                        if (subtreeGridColumn.CellDataFn(cellParams) is null)
                        {
                            nullGroup.Add(child);
                        }
                        else
                        {
                            if (!dic.ContainsKey(subtreeGridColumn.CellDataFn(cellParams).ToString()))
                            {
                                dic[subtreeGridColumn.CellDataFn(cellParams).ToString()] = groups.Count;
                                groups.Add(new List<TreeRowNode<TItem>>() {child});
                            }
                            else
                            {
                                groups[dic[subtreeGridColumn.CellDataFn(cellParams).ToString()]].Add(child);
                            }
                        }
                    }

                    tree.Children = new List<TreeRowNode<TItem>>();

                    if (nullGroup.Count > 0)
                        groups.Add(nullGroup);

                    foreach (var group in groups)
                    {
                        cellParams.RowData = group[0].Data.RowData;

                        tree.Children.Add(new TreeRowNode<TItem>()
                        {
                            Data = new RowNode<TItem>()
                            {
                                IsGroup = true,
                                Show = true,
                                AdvShow = true,
                                RowNodeId = GetId(ref lastId),
                            },
                            Children = group,
                            ColumnName = subtreeGridColumn.DataField,
                            Value = subtreeGridColumn.CellDataFn(cellParams) is null
                                ? null
                                : subtreeGridColumn.CellDataFn(cellParams).ToString()
                        });
                    }
                }
            }
            else
            {
                foreach (var child in tree.Children)
                    AuxRemoveGroupByColumn(child, gridColumn, gridColumns, ref lastId, cellParams);
            }

            return tree;
        }
    }
}