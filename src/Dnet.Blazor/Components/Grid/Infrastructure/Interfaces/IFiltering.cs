using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Interfaces
{
    public interface IFiltering<TItem>
    {
        TreeRowNode<TItem> FilterBy(TreeRowNode<TItem> tree, List<FilterModel> filters, List<GridColumn<TItem>> gridColumns, CellParams<TItem> cellParams);
    }
}
