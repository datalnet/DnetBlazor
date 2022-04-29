using Dnet.Blazor.Components.Grid.Infrastructure.Services;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Entities
{
    public class BaseColDefParams<TItem>
    {
        public RowNode<TItem> RowNode { get; set; }

        public GridColumn<TItem> GridColumn { get; set; }

        public TItem RowData { get; set; }

        public GridApi<TItem> GridApi { get; set; }
    }
}
