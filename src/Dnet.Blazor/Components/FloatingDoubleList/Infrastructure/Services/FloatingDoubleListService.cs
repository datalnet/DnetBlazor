using Dnet.Blazor.Components.FloatingDoubleList.Infrastructure.Models;

namespace Dnet.Blazor.Components.FloatingDoubleList.Infrastructure.Services;

public class FloatingDoubleListService<TItem>
{
    public event Action<TransferredItems<TItem>> OnSelectionChange;

    public void UpdateTransferredItems(TransferredItems<TItem> item)
    {
        OnSelectionChange?.Invoke(item);
    }
}
