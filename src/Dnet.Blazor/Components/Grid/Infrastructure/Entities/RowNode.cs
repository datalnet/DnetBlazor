using Dnet.Blazor.Components.Grid.BlgRow;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Entities
{
    public class RowNode<TItem>
    {
        public long RowNodeId { get; set; }

        private bool _selected { get; set; } = false;

        private bool _clicked { get; set; } = false;

        private bool _hovered { get; set; } = false;


        public bool Show { get; set; }

        public bool AdvShow { get; set; }

        public string GroupValue { get; set; }

        public TItem RowData { get; set; }

        public object RowDataValue { get; set; }

        public List<TItem> GroupData { get; set; }

        public List<TItem> AggregatedData { get; set; }

        public RowNode<TItem> Parent { get; set; }

        public int? Level { get; set; }

        public int? UiLevel { get; set; }

        public bool IsGroup { get; set; }

        public int? RowGroupIndex { get; set; }

        public bool LeafGroup { get; set; }

        public bool FirstChild { get; set; }

        public bool LastChild { get; set; }

        public int? ChildIndex { get; set; }

        public List<GridColumn<TItem>> RowGroupColumn { get; set; }

        public GridColumn<TItem> RowGridColumn { get; set; }

        public string KeyRowGroupColumn { get; set; }

        public List<RowNode<TItem>> ChildrenAfterGroup { get; set; }

        public List<RowNode<TItem>> ChildrenAfterFilter { get; set; }

        public List<RowNode<TItem>> ChildrenAfterSort { get; set; }

        public Dictionary<GridColumn<TItem>, bool> RowSpanSkippedCells { get; set; }

        public Dictionary<GridColumn<TItem>, uint> RowSpanTargetCells { get; set; }

        public int? AllChildrenCount { get; set; }

        public bool Expanded { get; set; }

        public int RowHeight { get; set; }

        public bool Selectable { get; set; } = true;

        public bool First { get; set; }

        public Dictionary<GridColumn<TItem>, uint> FirstSpanRow { get; set; }

        public Dictionary<GridColumn<TItem>, object> FirstSpanRowData { get; set; }

       
        public bool IsSelected() {

            return _selected;
        }

        public bool SelectThisNode(bool newValue) 
        {
            if (!Selectable || _selected == newValue) { return false; }

            _selected = newValue;

            return true;
        }

        public bool IsClicked()
        {
            return _clicked;
        }

        public bool ClickThisNode(bool newValue)
        {
            if (!Selectable || _clicked == newValue) { return false; }

            _clicked = newValue;

            return true;
        }

        public bool IsHovered()
        {
            return _hovered;
        }

        public bool HoverThisNode(bool newValue)
        {
            if (_hovered == newValue) { return false; }

            _hovered = newValue;

            return true;
        }
    }
}
