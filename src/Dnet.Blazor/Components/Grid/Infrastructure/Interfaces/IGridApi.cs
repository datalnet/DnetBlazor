using System.Collections.Generic;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Interfaces
{
    public interface IGridApi<TItem>
    {
        public void Init(List<GridColumn<TItem>> gridColumns);

        public List<GridColumn<TItem>> Reset();
    }
}
