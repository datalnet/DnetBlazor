using Dnet.Blazor.Components.Grid.Infrastructure.Enums;
using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Entities
{
    public class GridOptions<TItem>
    {
        public bool IsDebugMode { get; set; }

        public int HeaderHeight { get; set; } = 40;

        public int HeaderRowHeight { get; set; } = 40;

        public int RowHeight { get; set; } = 40;

        public List<string> RowStyle { get; set; } = new();

        public Func<CellParams<TItem>, List<string>>? RowStyleFn { get; set; }

        public List<string> RowClasses { get; set; } = new();

        public Func<CellParams<TItem>, List<string>>? RowClassFn { get; set; }

        public string? GridClass { get; set; }

        public bool EnableGrouping { get; set; }

        public bool EnableAdvancedFilter { get; set; }

        public FilterOperator DefaultAdvancedFilterOperator { get; set; } = FilterOperator.Contains;

        public bool EnableColResize { get; set; } = false;

        public bool EnableServerSideGrouping { get; set; } = false;

        public bool EnableSorting { get; set; } = false;

        public bool EnableServerSideSorting { get; set; } = false;

        public bool EnableFilter { get; set; } = false;

        public bool EnableServerSideFilter { get; set; } = false;

        public bool EnableServerSideAdvancedFilter { get; set; } = false;

        public bool EnableServerSidePagination { get; set; } = false;

        public bool Pagination { get; set; } = true;

        public bool SuppressPaginationPanel { get; set; }

        public int PaginationPageSize { get; set; } = 25;

        public int PaginationStartPage { get; set; } = 1;

        public int NumberOfRows { get; set; }

        public int? ColumnWidth { get; set; } = 200;

        public int? ColumnMinWidth { get; set; } = 200;

        public int? ColumnMaxWidth { get; set; }

        public RowSelectionType RowSelectionType { get; set; } = RowSelectionType.Single;

        public bool SuppressRowClickSelection { get; set; } = true;

        public bool RowMultiSelectWithClick { get; set; } = false;

        public bool SuppressRowDeselection { get; set; } = false;

        public bool GroupDefaultExpanded { get; set; } = true;

        public bool SuppressFilterRow { get; set; } = true;

        public bool CheckboxSelectionColumn { get; set; }

        internal bool CheckboxSelectionPinned { get; set; }

        public bool NullValueSortedToEnd { get; set; } = true;

        public int ScrollWidth { get; set; } = 6;

        public bool RowAlternateColorSchema { get; set; } = false;

        public bool UseVirtualization { get; set; } = true;

        public Func<TItem, bool>? DisableRow { get; set; }

        public bool ShowExpandCollapseButtons { get; set; } = true;
    }
}
