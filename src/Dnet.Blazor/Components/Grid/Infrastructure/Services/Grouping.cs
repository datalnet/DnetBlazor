using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services;

public class Grouping<TItem> : IGrouping<TItem>
{
    // Cache para evitar múltiples llamadas a CellDataFn con los mismos parámetros
    private readonly Dictionary<string, object> _cellDataCache = new Dictionary<string, object>();

    public TreeRowNode<TItem> AddGroupByColumn(TreeRowNode<TItem> tree,
        GridColumn<TItem> gridColumn,
        List<GridColumn<TItem>> gridColumns,
        ref long lastId,
        CellParams<TItem> cellParams)
    {
        // Clear cache for new operation
        _cellDataCache.Clear();

        // Si ya está agrupado por esta columna, no hacer nada
        if (gridColumn.RowGroup && gridColumn.RowGroupIndex != -1)
            return AuxAddGroupByColumn(tree, gridColumn, ref lastId, cellParams);

        // Optimizar búsqueda de columnas agrupadas usando LINQ
        var groupedGridColumns = gridColumns.Where(e => e.RowGroup).ToList();

        var lastLevel = groupedGridColumns.Count > 0 
            ? groupedGridColumns.Max(e => e.RowGroupIndex) 
            : -1;

        gridColumn.RowGroup = true;
        gridColumn.RowGroupIndex = lastLevel + 1;

        return AuxAddGroupByColumn(tree, gridColumn, ref lastId, cellParams);
    }

    private long GetId(ref long nextId)
    {
        return ++nextId;
    }

    public TreeRowNode<TItem> AuxAddGroupByColumn(TreeRowNode<TItem> tree,
        GridColumn<TItem> gridColumn,
        ref long lastId,
        CellParams<TItem> cellParams)
    {
        cellParams.GridColumn = gridColumn;
        cellParams.RowNode = tree.Data;

        if (tree.Data.IsGroup && tree.Children.Count > 0 && !tree.Children[0].Data.IsGroup)
        {
            GroupLeafNodes(tree, gridColumn, ref lastId, cellParams);
        }
        else
        {
            // Procesar recursivamente todos los hijos
            foreach (var child in tree.Children)
                AuxAddGroupByColumn(child, gridColumn, ref lastId, cellParams);
        }

        return tree;
    }

    private void GroupLeafNodes(TreeRowNode<TItem> tree, GridColumn<TItem> gridColumn, ref long lastId, CellParams<TItem> cellParams)
    {
        var groups = new Dictionary<string, List<TreeRowNode<TItem>>>();
        var nullGroup = new List<TreeRowNode<TItem>>();

        // Agrupar nodos hoja
        foreach (var child in tree.Children)
        {
            cellParams.RowData = child.Data.RowData;
            
            var cellValue = GetCachedCellData(gridColumn, cellParams);
            
            if (cellValue == null)
            {
                nullGroup.Add(child);
            }
            else
            {
                var groupKey = cellValue.ToString();
                
                if (!groups.TryGetValue(groupKey, out var group))
                {
                    group = new List<TreeRowNode<TItem>>();
                    groups[groupKey] = group;
                }
                group.Add(child);
            }
        }

        // Crear nuevos nodos de grupo
        var newChildren = new List<TreeRowNode<TItem>>(groups.Count + (nullGroup.Count > 0 ? 1 : 0));

        // Agregar grupos con valores
        foreach (var kvp in groups)
        {
            var groupValue = kvp.Key;
            var groupNodes = kvp.Value;
            
            // Usar el primer nodo para obtener el valor del grupo
            cellParams.RowData = groupNodes[0].Data.RowData;
            
            newChildren.Add(CreateGroupNode(groupValue, groupNodes, gridColumn, ref lastId));
        }

        // Agregar grupo null si existe
        if (nullGroup.Count > 0)
        {
            newChildren.Add(CreateGroupNode(null, nullGroup, gridColumn, ref lastId));
        }

        tree.Children = newChildren;
    }

    private TreeRowNode<TItem> CreateGroupNode(string groupValue, List<TreeRowNode<TItem>> children, GridColumn<TItem> gridColumn, ref long lastId)
    {
        return new TreeRowNode<TItem>
        {
            Data = new RowNode<TItem>
            {
                IsGroup = true,
                Show = true,
                AdvShow = true,
                RowNodeId = GetId(ref lastId),
            },
            Children = children,
            ColumnName = gridColumn.DataField,
            Value = groupValue
        };
    }

    private object GetCachedCellData(GridColumn<TItem> gridColumn, CellParams<TItem> cellParams)
    {
        // Crear una clave única para este contexto
        var cacheKey = $"{gridColumn.DataField}_{cellParams.RowData?.GetHashCode() ?? 0}";
        
        if (!_cellDataCache.TryGetValue(cacheKey, out var cachedValue))
        {
            cachedValue = gridColumn.CellDataFn(cellParams);
            _cellDataCache[cacheKey] = cachedValue;
        }
        
        return cachedValue;
    }

    public TreeRowNode<TItem> RemoveGroupByColumn(TreeRowNode<TItem> tree,
        GridColumn<TItem> gridColumn,
        List<GridColumn<TItem>> gridColumns,
        ref long lastId,
        CellParams<TItem> cellParams)
    {
        // Clear cache for new operation
        _cellDataCache.Clear();

        var rowGroupIndex = gridColumn.RowGroupIndex;

        gridColumn.RowGroup = false;
        gridColumn.RowGroupIndex = -1;

        // Ajustar índices de otras columnas agrupadas de forma más eficiente
        foreach (var gridCol in gridColumns.Where(g => g.RowGroup && g.RowGroupIndex > rowGroupIndex))
        {
            gridCol.RowGroupIndex--;
        }

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
            ProcessGroupRemoval(tree, gridColumn, gridColumns, ref lastId, cellParams);
        }
        else
        {
            // Procesar recursivamente todos los hijos
            foreach (var child in tree.Children)
                AuxRemoveGroupByColumn(child, gridColumn, gridColumns, ref lastId, cellParams);
        }

        return tree;
    }

    private void ProcessGroupRemoval(TreeRowNode<TItem> tree, GridColumn<TItem> gridColumn, List<GridColumn<TItem>> gridColumns, ref long lastId, CellParams<TItem> cellParams)
    {
        var firstChild = tree.Children[0];
        
        if (firstChild.Children.Count == 0)
        {
            // No hay más hijos, simplemente limpiar
            tree.Children.Clear();
            return;
        }

        var firstGrandChild = firstChild.Children[0];
        
        // Verificar si necesitamos reagrupar o simplemente aplanar
        if (!firstGrandChild.Data.IsGroup || 
            (firstGrandChild.Children.Count > 0 && firstGrandChild.Children[0].Data.IsGroup))
        {
            // Simple flattening
            FlattenGroups(tree);
        }
        else
        {
            // Reagrupar por la siguiente columna
            RegroupByNextColumn(tree, gridColumns, ref lastId, cellParams);
        }
    }

    private void FlattenGroups(TreeRowNode<TItem> tree)
    {
        var flattenedNodes = new List<TreeRowNode<TItem>>();
        
        foreach (var child in tree.Children)
        {
            flattenedNodes.AddRange(child.Children);
        }
        
        tree.Children = flattenedNodes;
    }

    private void RegroupByNextColumn(TreeRowNode<TItem> tree, List<GridColumn<TItem>> gridColumns, ref long lastId, CellParams<TItem> cellParams)
    {
        var firstGrandChild = tree.Children[0].Children[0];
        var subtreeGridColumn = gridColumns.FirstOrDefault(e => e.DataField == firstGrandChild.ColumnName);
        
        if (subtreeGridColumn == null)
        {
            FlattenGroups(tree);
            return;
        }

        var groups = new Dictionary<string, List<TreeRowNode<TItem>>>();
        var nullGroup = new List<TreeRowNode<TItem>>();

        cellParams.GridColumn = subtreeGridColumn;

        // Collect all leaf nodes from the nested structure
        foreach (var treeChild in tree.Children)
        {
            foreach (var subtreeChild in treeChild.Children)
            {
                foreach (var leafNode in subtreeChild.Children)
                {
                    cellParams.RowData = leafNode.Data.RowData;
                    
                    var cellValue = GetCachedCellData(subtreeGridColumn, cellParams);
                    
                    if (cellValue == null)
                    {
                        nullGroup.Add(leafNode);
                    }
                    else
                    {
                        var groupKey = cellValue.ToString();
                        
                        if (!groups.TryGetValue(groupKey, out var group))
                        {
                            group = new List<TreeRowNode<TItem>>();
                            groups[groupKey] = group;
                        }
                        group.Add(leafNode);
                    }
                }
            }
        }

        // Create new grouped structure
        var newChildren = new List<TreeRowNode<TItem>>(groups.Count + (nullGroup.Count > 0 ? 1 : 0));

        foreach (var kvp in groups)
        {
            var groupValue = kvp.Key;
            var groupNodes = kvp.Value;
            
            cellParams.RowData = groupNodes[0].Data.RowData;
            newChildren.Add(CreateGroupNode(groupValue, groupNodes, subtreeGridColumn, ref lastId));
        }

        if (nullGroup.Count > 0)
        {
            newChildren.Add(CreateGroupNode(null, nullGroup, subtreeGridColumn, ref lastId));
        }

        tree.Children = newChildren;
    }
}