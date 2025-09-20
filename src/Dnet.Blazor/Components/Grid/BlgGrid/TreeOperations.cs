using Dnet.Blazor.Components.Grid.Infrastructure.Entities;

namespace Dnet.Blazor.Components.Grid.BlgGrid;

public partial class BlgGrid<TItem>
{
    private TreeRowNode<TItem> BuildRowNodes()
    {
        var treeRn = InitializeTree();

        foreach (var data in _gridData)
        {
            treeRn.Children.Add(new TreeRowNode<TItem>
            {
                Data = new RowNode<TItem>
                {
                    RowNodeId = GetId(),
                    RowData = data,
                    Show = true,
                    Expanded = true,
                    AdvShow = true
                },
                Children = new List<TreeRowNode<TItem>>(),
                ColumnName = ""
            });
        }

        return treeRn;
    }

    private List<RowNode<TItem>> FlattenTree(TreeRowNode<TItem> tree, int level)
    {
        var rowNodeList = new List<RowNode<TItem>>();

        if (tree.Data.IsGroup)
        {
            tree.Data.GroupValue = tree.Value;
            tree.Data.Level = level;
        }

        rowNodeList.Add(tree.Data);

        if (!tree.Data.Expanded) return rowNodeList;

        foreach (var child in tree.Children)
        {
            rowNodeList.AddRange(FlattenTree(child, level + 1));
        }

        return rowNodeList;
    }

    private long GetId()
    {
        _nextId++;
        return _nextId;
    }

    private TreeRowNode<TItem> SelectTreeRowNode(TreeRowNode<TItem> tree, bool select)
    {
        tree.Data.SelectThisNode(select);

        foreach (var subtree in tree.Children)
            SelectTreeRowNode(subtree, select);

        return tree;
    }

    private TreeRowNode<TItem> ExpandCollapseTreeRowNode(TreeRowNode<TItem> tree, bool expandCollapse)
    {
        tree.Data.Expanded = expandCollapse;

        foreach (var subtree in tree.Children)
            ExpandCollapseTreeRowNode(subtree, expandCollapse);

        return tree;
    }

    private void AuxChangeSelectAllNodes(TreeRowNode<TItem> tree, bool value)
    {
        var disabledRow = false;

        if (tree.Data.RowData != null)
        {
            disabledRow = GridOptions.DisableRow != null && GridOptions?.DisableRow(tree.Data.RowData) == true ? true : false;
        }

        tree.Data.SelectThisNode(value && disabledRow == false);

        foreach (var subtree in tree.Children)
            AuxChangeSelectAllNodes(subtree, value);
    }
}