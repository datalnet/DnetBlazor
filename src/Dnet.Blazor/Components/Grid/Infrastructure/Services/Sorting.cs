using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Infrastructure.Models.SearchModels;
using System.Globalization;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services;

public class Sorting<TItem> : ISorting<TItem>
{
    private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

    public void UpdateOrder(List<GridColumn<TItem>> gridColumns, GridColumn<TItem> gridColumn)
    {
        var sortStatus = gridColumn.SortStatus;

        // Optimización: usar LINQ en lugar de ForEach
        foreach (var col in gridColumns)
            col.SortStatus = SortOrder.None;

        gridColumn.SortStatus = sortStatus switch
        {
            SortOrder.None => SortOrder.Ascending,
            SortOrder.Ascending => SortOrder.Descending,
            SortOrder.Descending => SortOrder.None,
            _ => gridColumn.SortStatus
        };
    }

    private readonly struct SortItem<T> : IComparable<SortItem<T>> where T : IComparable<T>
    {
        public readonly T Value;
        public readonly int Position;
        public readonly int SortOrder;

        public SortItem(T value, int position, int sortOrder)
        {
            Value = value;
            Position = position;
            SortOrder = sortOrder;
        }

        public int CompareTo(SortItem<T> other)
        {
            var result = Value.CompareTo(other.Value);
            return result * SortOrder;
        }
    }

    public TreeRowNode<TItem> SortBy(TreeRowNode<TItem> tree, GridColumn<TItem> gridColumn, CellParams<TItem> cellParams)
    {
        if (tree.Children.Count > 0 && !tree.Children[0].Data.IsGroup)
        {
            tree.Children = SortNodes(tree.Children, gridColumn, false, cellParams);
        }
        else
        {
            // Procesar recursivamente todos los hijos
            foreach (var child in tree.Children)
                SortBy(child, gridColumn, cellParams);
        }

        return tree;
    }

    public TreeRowNode<TItem> SortGroupingBy(TreeRowNode<TItem> tree, GridColumn<TItem> gridColumn, CellParams<TItem> cellParams)
    {
        if (tree.Children.Count > 0 && tree.Children[0].ColumnName == gridColumn.DataField)
        {
            tree.Children = SortNodes(tree.Children, gridColumn, true, cellParams);
        }
        else
        {
            // Procesar recursivamente todos los hijos
            foreach (var child in tree.Children)
                SortGroupingBy(child, gridColumn, cellParams);
        }
        
        return tree;
    }

    private List<TreeRowNode<TItem>> SortNodes(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
    {
        if (nodes.Count <= 1)
            return nodes;

        return gridColumn.CellDataType switch
        {
            CellDataType.Number => SortNumbers(nodes, gridColumn, isGroup, cellParams),
            CellDataType.Text => SortTexts(nodes, gridColumn, isGroup, cellParams),
            CellDataType.Date => SortDates(nodes, gridColumn, isGroup, cellParams),
            CellDataType.Boolean => SortBooleans(nodes, gridColumn, isGroup, cellParams),
            _ => nodes
        };
    }

    private List<TreeRowNode<TItem>> SortNumbers(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
    {
        var sortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1;
        cellParams.GridColumn = gridColumn;

        if (gridColumn.IsDecimalCellDataType)
        {
            return SortByType<decimal>(nodes, gridColumn, isGroup, cellParams, sortOrder, 
                data => ParseDecimal(data), decimal.MinValue);
        }
        else
        {
            return SortByType<long>(nodes, gridColumn, isGroup, cellParams, sortOrder, 
                data => ParseLong(data), long.MinValue);
        }
    }

    private List<TreeRowNode<TItem>> SortTexts(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
    {
        var sortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1;
        cellParams.GridColumn = gridColumn;

        return SortByType<string>(nodes, gridColumn, isGroup, cellParams, sortOrder, 
            data => data?.ToString() ?? string.Empty, string.Empty);
    }

    private List<TreeRowNode<TItem>> SortDates(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
    {
        var sortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1;
        cellParams.GridColumn = gridColumn;

        return SortByType<DateTime>(nodes, gridColumn, isGroup, cellParams, sortOrder, 
            data => ParseDateTime(data), DateTime.MinValue);
    }

    private List<TreeRowNode<TItem>> SortBooleans(List<TreeRowNode<TItem>> nodes, GridColumn<TItem> gridColumn, bool isGroup, CellParams<TItem> cellParams)
    {
        var sortOrder = gridColumn.SortStatus == SortOrder.Descending ? -1 : 1;
        cellParams.GridColumn = gridColumn;

        return SortByType<bool>(nodes, gridColumn, isGroup, cellParams, sortOrder, 
            data => ParseBoolean(data), false);
    }

    private List<TreeRowNode<TItem>> SortByType<T>(
        List<TreeRowNode<TItem>> nodes, 
        GridColumn<TItem> gridColumn, 
        bool isGroup, 
        CellParams<TItem> cellParams,
        int sortOrder,
        Func<object, T> parseFunc,
        T defaultValue) where T : IComparable<T>
    {
        // Crear array de items para sort más eficiente
        var sortItems = new SortItem<T>[nodes.Count];
        
        for (var i = 0; i < nodes.Count; i++)
        {
            cellParams.RowData = nodes[i].Data.RowData;
            
            var data = isGroup ? nodes[i].Value : gridColumn.CellDataFn(cellParams);
            var value = data == null ? defaultValue : parseFunc(data);
            
            sortItems[i] = new SortItem<T>(value, i, sortOrder);
        }

        // Sort in-place para mejor rendimiento
        Array.Sort(sortItems);

        // Crear lista resultado con capacidad exacta
        var sortedNodes = new List<TreeRowNode<TItem>>(nodes.Count);
        
        foreach (var item in sortItems)
        {
            sortedNodes.Add(nodes[item.Position]);
        }

        return sortedNodes;
    }

    // Métodos de parsing optimizados con manejo de errores
    private static decimal ParseDecimal(object data)
    {
        if (data == null) return decimal.MinValue;
        
        var dataString = data.ToString();
        if (string.IsNullOrWhiteSpace(dataString)) return decimal.MinValue;
        
        // Manejar comas como separadores decimales
        dataString = dataString.Replace(',', '.');
        
        return decimal.TryParse(dataString, NumberStyles.Float, InvariantCulture, out var result) 
            ? result 
            : decimal.MinValue;
    }

    private static long ParseLong(object data)
    {
        if (data == null) return long.MinValue;
        
        var dataString = data.ToString();
        if (string.IsNullOrWhiteSpace(dataString)) return long.MinValue;
        
        return long.TryParse(dataString, NumberStyles.Integer, InvariantCulture, out var result) 
            ? result 
            : long.MinValue;
    }

    private static DateTime ParseDateTime(object data)
    {
        if (data == null) return DateTime.MinValue;
        
        var dataString = data.ToString();
        if (string.IsNullOrWhiteSpace(dataString)) return DateTime.MinValue;
        
        return DateTime.TryParse(dataString, InvariantCulture, DateTimeStyles.None, out var result) 
            ? result 
            : DateTime.MinValue;
    }

    private static bool ParseBoolean(object data)
    {
        if (data == null) return false;
        
        var dataString = data.ToString();
        if (string.IsNullOrWhiteSpace(dataString)) return false;
        
        return bool.TryParse(dataString, out var result) && result;
    }
}