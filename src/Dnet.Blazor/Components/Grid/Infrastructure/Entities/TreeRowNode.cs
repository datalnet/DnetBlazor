using System.Collections.Generic;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Entities
{
    public class TreeRowNode<TItem>
    {
        public RowNode<TItem> Data { get; set; }

        public List<TreeRowNode<TItem>> Children { get; set; }

        public string ColumnName { get; set; }

        public string Value { get; set; }        
    }
}
