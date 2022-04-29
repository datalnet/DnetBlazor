namespace Dnet.Blazor.Components.Tree
{
    public class TreeNodeModel<TNode>
    {
        private bool _selected { get; set; } = false;
        
        private bool _active { get; set; } = false;

        private bool _expanded { get; set; } = false;

        public bool Show { get; set; }

        public long RowNodeId { get; set; }

        public TNode TreeNodeData { get; set; }

        public List<TreeNodeModel<TNode>> Children { get; set; }

        public bool Selectable { get; set; } = true;

        public bool IsSelected() {

            return _selected;
        }

        public bool SelectThisNode(bool newValue) 
        {
            if (!Selectable || _selected == newValue) { return false; }

            _selected = newValue;

            return true;
        }
        
        public bool IsActive() {

            return _active;
        }
        
        public bool ActiveThisNode(bool newValue) 
        {
            if (_active == newValue) { return false; }

            _active = newValue;

            return true;
        }

        public bool IsExpanded()
        {

            return _expanded && Children.Count > 0;
        }

        public bool ExpandNode(bool newValue)
        {
            if (_expanded == newValue) { return false; }

            _expanded = newValue;

            return true;
        }
    }
}
