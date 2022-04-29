using System.Collections.Generic;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Interfaces
{
    public interface IGrouping<TItem>
    {
        TreeRowNode<TItem> AddGroupByColumn(TreeRowNode<TItem> tree, 
                                            GridColumn<TItem> gridColumn, 
                                            List<GridColumn<TItem>> gridColumns, 
                                            ref long lastId, 
                                            CellParams<TItem> cellParams);

        TreeRowNode<TItem> RemoveGroupByColumn(TreeRowNode<TItem> tree, 
                                               GridColumn<TItem> gridColumn, 
                                               List<GridColumn<TItem>> gridColumns, 
                                               ref long lastId, 
                                               CellParams<TItem> cellParams);
    }
}
