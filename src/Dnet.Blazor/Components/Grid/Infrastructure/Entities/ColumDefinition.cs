using Dnet.Blazor.Components.Grid.Infrastructure.Enums;
using Dnet.Blazor.Infrastructure.Models.SearchModels;
using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Entities
{
    public class GridColumn<TItem>
    {
        public int ColumnId { get; set; }

        public int ColumnOrder { get; set; }

        public CellDataType CellDataType { get; set; }

        public string HeaderName { get; set; }

        public bool WrapHeaderText { get; set; }

        public List<string> HeaderClass { get; set; }

        public string DataField { get; set; }

        public bool Hide { get; set; }

        public int Width { get; set; }

        public int? MinWidth { get; set; }

        public int? MaxWidth { get; set; }

        public int? Height { get; set; }

        public string AlingContent { get; set; } = "flex-start";

        public Func<CellParams<TItem>, string> CellClassFn { get; set; }

        public Func<CellParams<TItem>, string> CellStyleFn { get; set; }

        public bool Resizable { get; set; }

        public int CanGrow { get; set; } = 1;

        public int CanShrink { get; set; } = 0;

        public bool Editable { get; set; }

        public bool RowGroup { get; set; }

        public bool ShowGroupingButtons { get; set; }

        public int RowGroupIndex { get; set; } = -1;

        public bool EnableSimpleFilter { get; set; }

        public bool EnableAdvancedFilter { get; set; } = false;

        public string Filter { get; set; } = "";

        public string DateFormat { get; set; } = "yyyy-MM-dd";

        public AdvancedFilterModel AdvancedFilterModel { get; set; } = new();

        public Func<CellParams<TItem>, bool> CheckboxSelectionFn { get; set; }

        public bool HeaderCheckboxSelectionFilteredOnly { get; set; }

        public bool Sortable { get; set; }

        public bool WrapCellText { get; set; } = false;

        public SortOrder SortStatus { get; set; } = SortOrder.None;

        public Pinned Pinned { get; set; } = Pinned.None;

        public Func<CellParams<TItem>, object> CellDataFn { get; set; }

        public Func<CellParams<TItem>, uint> ColumnSpanFn { get; set; }

        public Func<CellParams<TItem>, uint> RowSpanFn { get; set; }
        
        public Func<CellParams<TItem>, RenderFragment> CellTemplateFn { get; set; }

        public Type CellTemplate { get; set; }

        public bool IsSortAscending()
        {
            return SortStatus == SortOrder.Ascending;
        }

        public bool IsSortDescending()
        {
            return SortStatus == SortOrder.Descending;
        }

        public bool IsSortNone()
        {
            return SortStatus == SortOrder.None;
        }        

        public GridColumn()
        {           
        }

        public GridColumn(GridColumn<TItem> gridColumn)
        {
            CanGrow = gridColumn.CanGrow;
            CellDataFn = gridColumn.CellDataFn;
            ColumnId = gridColumn.ColumnId;
            CellTemplate = gridColumn.CellTemplate;
            CheckboxSelectionFn = gridColumn.CheckboxSelectionFn;
            DataField = gridColumn.DataField;
            Editable = gridColumn.Editable;
            Filter = gridColumn.Filter;
            AdvancedFilterModel = new AdvancedFilterModel
            {
                AdditionalOperator = gridColumn.AdvancedFilterModel.AdditionalOperator,
                AdditionalValue = gridColumn.AdvancedFilterModel.AdditionalValue,
                Column = gridColumn.AdvancedFilterModel.Column,
                Condition = gridColumn.AdvancedFilterModel.Condition,
                Operator = gridColumn.AdvancedFilterModel.Operator,
                Type = gridColumn.AdvancedFilterModel.Type,
                Value = gridColumn.AdvancedFilterModel.Value
            };
            CellDataType = gridColumn.CellDataType;
            HeaderCheckboxSelectionFilteredOnly = gridColumn.HeaderCheckboxSelectionFilteredOnly;
            HeaderClass = gridColumn.HeaderClass;
            HeaderName = gridColumn.HeaderName;
            Height = gridColumn.Height;
            Hide = gridColumn.Hide;
            MaxWidth = gridColumn.MaxWidth; 
            MinWidth = gridColumn.MinWidth;
            Resizable = gridColumn.Resizable;
            RowGroup = gridColumn.RowGroup;
            RowGroupIndex = gridColumn.RowGroupIndex;
            Sortable = gridColumn.Sortable;
            SortStatus = gridColumn.SortStatus;
            Width = gridColumn.Width;
        }
    }
}
