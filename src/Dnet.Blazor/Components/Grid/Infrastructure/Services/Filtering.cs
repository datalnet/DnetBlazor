using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Infrastructure.Models.SearchModels;
using System.Globalization;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class Filtering<TItem> : IFiltering<TItem>
    {
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
        private static readonly StringComparison OrdinalIgnoreCase = StringComparison.OrdinalIgnoreCase;

        public TreeRowNode<TItem> FilterBy(TreeRowNode<TItem> tree, 
                                           List<FilterModel> filters,
                                           List<GridColumn<TItem>> gridColumns, 
                                           CellParams<TItem> cellParams)
        {
            // IMPORTANTE: Resetear el estado de visibilidad antes de aplicar filtros
            ResetVisibilityState(tree);

            // Early return si no hay filtros - pero después del reset
            if (filters == null || filters.Count == 0)
                return tree;

            // Pre-compilar los filtros para evitar búsquedas repetitivas
            var compiledFilters = PrecompileFilters(filters, gridColumns);

            // Solo procesar si hay filtros válidos
            if (compiledFilters.Count > 0)
            {
                foreach (var child in tree.Children)
                    Show(child, compiledFilters, cellParams);
            }

            return tree;
        }

        private void ResetVisibilityState(TreeRowNode<TItem> tree)
        {
            // Resetear el estado de este nodo
            if (tree.Data != null)
            {
                tree.Data.Show = true;
            }

            // Resetear recursivamente todos los hijos
            if (tree.Children != null)
            {
                foreach (var child in tree.Children)
                {
                    ResetVisibilityState(child);
                }
            }
        }

        private List<CompiledFilter> PrecompileFilters(List<FilterModel> filters, List<GridColumn<TItem>> gridColumns)
        {
            var compiledFilters = new List<CompiledFilter>(filters.Count);
            var columnDictionary = gridColumns.ToDictionary(col => col.DataField, col => col);

            foreach (var filter in filters)
            {
                // Verificar que el filtro tenga valor antes de procesarlo
                if (string.IsNullOrWhiteSpace(filter.Filter))
                    continue;

                if (columnDictionary.TryGetValue(filter.DataDield, out var gridColumn))
                {
                    var compiledFilter = new CompiledFilter
                    {
                        GridColumn = gridColumn,
                        OriginalFilter = filter.Filter
                    };

                    // Pre-procesar el filtro según el tipo de datos
                    switch (gridColumn.CellDataType)
                    {
                        case CellDataType.Text:
                        case CellDataType.Number:
                        case CellDataType.Boolean:
                            compiledFilter.ProcessedFilter = filter.Filter.ToUpperInvariant();
                            compiledFilter.FilterType = FilterType.Contains;
                            break;

                        case CellDataType.Date:
                            if (DateTime.TryParse(filter.Filter, InvariantCulture, DateTimeStyles.None, out var filterDate))
                            {
                                compiledFilter.FilterDate = filterDate;
                                compiledFilter.ProcessedFilter = filterDate.ToString(gridColumn.DateFormat ?? "yyyy-MM-dd");
                                compiledFilter.FilterType = FilterType.DateEquals;
                            }
                            else
                            {
                                continue; // Saltar filtros de fecha inválidos
                            }
                            break;

                        default:
                            continue; // Saltar tipos no soportados
                    }

                    compiledFilters.Add(compiledFilter);
                }
            }

            return compiledFilters;
        }

        private bool Show(TreeRowNode<TItem> tree, 
                          List<CompiledFilter> compiledFilters,
                          CellParams<TItem> cellParams)
        {
            if (!tree.Data.IsGroup)
            {
                // Aplicar filtros a nodos hoja
                tree.Data.Show = ApplyFiltersToLeaf(tree, compiledFilters, cellParams);
                return tree.Data.Show;
            }
            else
            {
                // Para grupos, el grupo es visible si al menos un hijo es visible
                bool hasVisibleChild = false;
                
                foreach (var child in tree.Children)
                {
                    if (Show(child, compiledFilters, cellParams))
                    {
                        hasVisibleChild = true;
                        // No hacer break aquí - necesitamos procesar todos los hijos
                        // para que tengan su estado Show correcto
                    }
                }
                
                tree.Data.Show = hasVisibleChild;
                return tree.Data.Show;
            }
        }

        private bool ApplyFiltersToLeaf(TreeRowNode<TItem> tree, 
                                        List<CompiledFilter> compiledFilters, 
                                        CellParams<TItem> cellParams)
        {
            // Configurar cellParams una vez para este nodo
            cellParams.RowData = tree.Data.RowData;
            cellParams.RowNode = tree.Data;

            // Todos los filtros deben pasar (AND lógico)
            foreach (var filter in compiledFilters)
            {
                cellParams.GridColumn = filter.GridColumn;

                var cellValue = filter.GridColumn.CellDataFn(cellParams);
                if (cellValue == null)
                    return false;

                if (!EvaluateFilter(cellValue, filter))
                    return false; // Short-circuit: si un filtro falla, no continuar
            }

            return true;
        }

        private bool EvaluateFilter(object cellValue, CompiledFilter filter)
        {
            var cellDataString = cellValue.ToString();
            
            if (string.IsNullOrEmpty(cellDataString))
                return false;

            return filter.FilterType switch
            {
                FilterType.Contains => EvaluateContainsFilter(cellDataString, filter.ProcessedFilter),
                FilterType.DateEquals => EvaluateDateFilter(cellDataString, filter),
                _ => true
            };
        }

        private bool EvaluateContainsFilter(string cellData, string processedFilter)
        {
            // Usar IndexOf con StringComparison para mejor rendimiento que Contains
            return cellData.ToUpperInvariant().IndexOf(processedFilter, OrdinalIgnoreCase) >= 0;
        }

        private bool EvaluateDateFilter(string cellData, CompiledFilter filter)
        {
            // Intentar parsear la fecha de la celda
            if (!DateTime.TryParse(cellData, InvariantCulture, DateTimeStyles.None, out var cellDate))
                return false;

            // Comparar usando el formato especificado para evitar problemas de precisión
            var cellDateFormatted = cellDate.ToString(filter.GridColumn.DateFormat ?? "yyyy-MM-dd");
            
            return string.Equals(cellDateFormatted, filter.ProcessedFilter, OrdinalIgnoreCase);
        }

        private class CompiledFilter
        {
            public GridColumn<TItem> GridColumn { get; set; }
            public string OriginalFilter { get; set; }
            public string ProcessedFilter { get; set; }
            public DateTime? FilterDate { get; set; }
            public FilterType FilterType { get; set; }
        }

        private enum FilterType
        {
            Contains,
            DateEquals
        }
    }
}