using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Infrastructure.Models.SearchModels;
using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;
using System.Globalization;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class AdvancedFiltering<TItem> : IAdvancedFiltering<TItem>
    {
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
        private static readonly StringComparison OrdinalIgnoreCase = StringComparison.OrdinalIgnoreCase;

        public List<GridColumn<TItem>> InitAdvancedFilterModels(List<GridColumn<TItem>> gridColumns, FilterOperator defaultAdvancedFilterOperator)
        {
            foreach (var gridColumn in gridColumns)
            {
                if (gridColumn.AdvancedFilterModel.Column == null && 
                    gridColumn.CellDataType != CellDataType.None && 
                    gridColumn.EnableAdvancedFilter)
                {
                    gridColumn.AdvancedFilterModel = new AdvancedFilterModel
                    {
                        Operator = gridColumn.CellDataType == CellDataType.Text ? defaultAdvancedFilterOperator : FilterOperator.None,
                        AdditionalOperator = FilterOperator.None,
                        Type = gridColumn.CellDataType,
                        Column = gridColumn.DataField,
                        Value = string.Empty,
                        AdditionalValue = string.Empty
                    };
                }
            }
            return gridColumns;
        }

        public TreeRowNode<TItem> FilterBy(TreeRowNode<TItem> tree, List<AdvancedFilterModel> advancedFilterList, List<GridColumn<TItem>> gridColumns, CellParams<TItem> cellParams)
        {
            // IMPORTANTE: Resetear el estado de visibilidad antes de aplicar filtros
            ResetAdvancedVisibilityState(tree);

            // Early return si no hay filtros - pero después del reset
            if (advancedFilterList == null || advancedFilterList.Count == 0)
                return tree;

            // Crear diccionario de columnas para búsquedas rápidas
            var columnDictionary = gridColumns.ToDictionary(col => col.DataField, col => col);

            // Filtrar solo los filtros válidos
            var validFilters = advancedFilterList.Where(f => IsValidFilter(f, columnDictionary)).ToList();

            if (validFilters.Count > 0)
            {
                foreach (var child in tree.Children)
                    ProcessNode(child, validFilters, columnDictionary, cellParams);
            }
            
            return tree;
        }

        private void ResetAdvancedVisibilityState(TreeRowNode<TItem> tree)
        {
            // Resetear el estado de este nodo
            if (tree.Data != null)
            {
                tree.Data.AdvShow = true;
            }

            // Resetear recursivamente todos los hijos
            if (tree.Children != null)
            {
                foreach (var child in tree.Children)
                {
                    ResetAdvancedVisibilityState(child);
                }
            }
        }

        private bool IsValidFilter(AdvancedFilterModel filter, Dictionary<string, GridColumn<TItem>> columnDictionary)
        {
            if (!columnDictionary.ContainsKey(filter.Column))
                return false;

            // Un filtro es válido si tiene al menos un valor y un operador
            bool hasFirstPart = !string.IsNullOrWhiteSpace(filter.Value) && filter.Operator != FilterOperator.None;
            bool hasSecondPart = !string.IsNullOrWhiteSpace(filter.AdditionalValue) && filter.AdditionalOperator != FilterOperator.None;

            return hasFirstPart || hasSecondPart;
        }

        private bool ProcessNode(TreeRowNode<TItem> tree, List<AdvancedFilterModel> validFilters, Dictionary<string, GridColumn<TItem>> columnDictionary, CellParams<TItem> cellParams)
        {
            if (!tree.Data.IsGroup)
            {
                // Nodo hoja: evaluar todos los filtros
                tree.Data.AdvShow = EvaluateAllFilters(tree, validFilters, columnDictionary, cellParams);
                return tree.Data.AdvShow;
            }
            else
            {
                // Nodo grupo: es visible si al menos un hijo es visible
                bool hasVisibleChild = false;
                
                foreach (var child in tree.Children)
                {
                    if (ProcessNode(child, validFilters, columnDictionary, cellParams))
                    {
                        hasVisibleChild = true;
                        // NO hacer break - necesitamos procesar todos los hijos
                    }
                }
                
                tree.Data.AdvShow = hasVisibleChild;
                return tree.Data.AdvShow;
            }
        }

        private bool EvaluateAllFilters(TreeRowNode<TItem> tree, List<AdvancedFilterModel> validFilters, Dictionary<string, GridColumn<TItem>> columnDictionary, CellParams<TItem> cellParams)
        {
            // TODOS los filtros deben pasar (AND lógico entre filtros)
            foreach (var filter in validFilters)
            {
                if (!EvaluateSingleFilter(tree, filter, columnDictionary, cellParams))
                {
                    return false; // Short-circuit: si un filtro falla, el nodo no pasa
                }
            }
            
            return true;
        }

        private bool EvaluateSingleFilter(TreeRowNode<TItem> tree, AdvancedFilterModel filter, Dictionary<string, GridColumn<TItem>> columnDictionary, CellParams<TItem> cellParams)
        {
            var gridColumn = columnDictionary[filter.Column];
            
            // Configurar cellParams para esta evaluación
            cellParams.RowData = tree.Data.RowData;
            cellParams.GridColumn = gridColumn;
            cellParams.RowNode = tree.Data;

            // Evaluar primera parte del filtro
            bool firstPartResult = true;
            if (!string.IsNullOrWhiteSpace(filter.Value) && filter.Operator != FilterOperator.None)
            {
                firstPartResult = EvaluateFilterPart(gridColumn, cellParams, filter.Value, filter.Operator, filter.Type);
            }

            // Evaluar segunda parte del filtro (si existe)
            bool secondPartResult = true;
            if (!string.IsNullOrWhiteSpace(filter.AdditionalValue) && filter.AdditionalOperator != FilterOperator.None)
            {
                secondPartResult = EvaluateFilterPart(gridColumn, cellParams, filter.AdditionalValue, filter.AdditionalOperator, filter.Type);
            }

            // Combinar resultados según la condición
            return filter.Condition switch
            {
                FilterCondition.None => firstPartResult,
                FilterCondition.And => firstPartResult && secondPartResult,
                FilterCondition.Or => firstPartResult || secondPartResult,
                _ => true
            };
        }

        private bool EvaluateFilterPart(GridColumn<TItem> gridColumn, CellParams<TItem> cellParams, string filterValue, FilterOperator filterOperator, CellDataType cellDataType)
        {
            var cellValue = gridColumn.CellDataFn(cellParams);
            
            if (cellValue == null)
                return false;

            var cellDataString = cellValue.ToString();
            
            if (string.IsNullOrEmpty(cellDataString))
                return false;

            return cellDataType switch
            {
                CellDataType.Text => ApplyTextFilter(cellDataString, filterValue, filterOperator),
                CellDataType.Number => ApplyNumberFilter(cellDataString, filterValue, filterOperator),
                CellDataType.Boolean => ApplyBooleanFilter(cellDataString, filterValue),
                CellDataType.Date => ApplyDateFilter(cellDataString, filterValue, filterOperator, gridColumn.DateFormat),
                _ => true
            };
        }

        private bool ApplyTextFilter(string cellData, string filterValue, FilterOperator filterOperator)
        {
            var cellDataLower = cellData.ToLowerInvariant();
            var filterValueLower = filterValue.ToLowerInvariant();

            return filterOperator switch
            {
                FilterOperator.Contains => cellDataLower.Contains(filterValueLower, OrdinalIgnoreCase),
                FilterOperator.Equals => string.Equals(cellDataLower, filterValueLower, OrdinalIgnoreCase),
                FilterOperator.NotContains => !cellDataLower.Contains(filterValueLower, OrdinalIgnoreCase),
                FilterOperator.NotEquals => !string.Equals(cellDataLower, filterValueLower, OrdinalIgnoreCase),
                FilterOperator.StartsWith => cellDataLower.StartsWith(filterValueLower, OrdinalIgnoreCase),
                FilterOperator.EndsWith => cellDataLower.EndsWith(filterValueLower, OrdinalIgnoreCase),
                _ => true
            };
        }

        private bool ApplyNumberFilter(string cellData, string filterValue, FilterOperator filterOperator)
        {
            if (!int.TryParse(cellData, NumberStyles.Integer, InvariantCulture, out var cellNumber) || 
                !int.TryParse(filterValue, NumberStyles.Integer, InvariantCulture, out var filterNumber))
                return false;

            return filterOperator switch
            {
                FilterOperator.Equals => cellNumber == filterNumber,
                FilterOperator.NotEquals => cellNumber != filterNumber,
                FilterOperator.GreaterThan => cellNumber > filterNumber,
                FilterOperator.LessThan => cellNumber < filterNumber,
                FilterOperator.GreaterThanOrEqual => cellNumber >= filterNumber,
                FilterOperator.LessThanOrEqual => cellNumber <= filterNumber,
                _ => true
            };
        }

        private bool ApplyBooleanFilter(string cellData, string filterValue)
        {
            return cellData.Contains(filterValue, OrdinalIgnoreCase);
        }

        private bool ApplyDateFilter(string cellData, string filterValue, FilterOperator filterOperator, string dateFormat)
        {
            if (!DateTime.TryParse(cellData, InvariantCulture, DateTimeStyles.None, out var cellDate) || 
                !DateTime.TryParse(filterValue, InvariantCulture, DateTimeStyles.None, out var filterDate))
                return false;

            return filterOperator switch
            {
                FilterOperator.Equals => cellDate.Date == filterDate.Date,
                FilterOperator.NotEquals => cellDate.Date != filterDate.Date,
                FilterOperator.GreaterThan => cellDate > filterDate,
                FilterOperator.LessThan => cellDate < filterDate,
                FilterOperator.GreaterThanOrEqual => cellDate >= filterDate,
                FilterOperator.LessThanOrEqual => cellDate <= filterDate,
                _ => true
            };
        }
    }
}