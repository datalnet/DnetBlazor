using System.Collections.Generic;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Interfaces
{
    public interface ISorting<TItem>
    {
        void UpdateOrder(List<GridColumn<TItem>> gridColumns, GridColumn<TItem> gridColumn);

        TreeRowNode<TItem> SortBy(TreeRowNode<TItem> Tree, GridColumn<TItem> gridColumn, CellParams<TItem> cellParams);

        TreeRowNode<TItem> SortGroupingBy(TreeRowNode<TItem> Tree, GridColumn<TItem> gridColumn, CellParams<TItem> cellParams);
    }
}
