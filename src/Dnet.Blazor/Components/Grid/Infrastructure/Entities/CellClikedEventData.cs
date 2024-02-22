using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Entities
{
    public class CellClikedData<TItem>
    {
        public RowNode<TItem>? RowNode { get; set; }

        public int ColumnId { get; set; }

        public int ColumnOrder { get; set; }

        public string? HeaderName { get; set; }

        public string? DataField { get; set; }

        public AdvancedFilterModel AdvancedFilterModel { get; set; } = new();
    }
}
