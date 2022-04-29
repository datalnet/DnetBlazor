using System.Collections.Generic;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class GridApi<TItem>
    {
        private List<RowNode<TItem>> _rowNodes { get; set; }

        private List<RowNode<TItem>> _renderedRowNodes { get; set; }

        private TreeRowNode<TItem> _treeRowNodes { get; set; }

        internal List<RowNode<TItem>> RowNodes
        {
            get => _rowNodes;
            set
            {
                _rowNodes = value; ;
            }
        }

        internal List<RowNode<TItem>> RenderedRowNodes
        {
            get => _renderedRowNodes;
            set
            {
                _renderedRowNodes = value; ;
            }
        }

        internal TreeRowNode<TItem> TreeRowNodes
        {
            get => _treeRowNodes;
            set
            {
                _treeRowNodes = value; ;
            }
        }

        public IEnumerable<RowNode<TItem>> GetRowNodes()
        {
            return _rowNodes;
        }

        public IEnumerable<RowNode<TItem>> GetRenderedRowNodes()
        {
            return _rowNodes;
        }

        public TreeRowNode<TItem> GetTreeRowNodes()
        {
            return _treeRowNodes;
        }
    }
}
