using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Interfaces
{
    public interface IAdvancedFiltering<TItem>
    {
        public List<GridColumn<TItem>> InitAdvancedFilterModels(List<GridColumn<TItem>> GridColumns, FilterOperator DefaultAdvancedFilterOperator);

        TreeRowNode<TItem> FilterBy(TreeRowNode<TItem> Tree, List<AdvancedFilterModel> advancedFilterList, List<GridColumn<TItem>> GridColumns, CellParams<TItem> cellParams);
    }
}
