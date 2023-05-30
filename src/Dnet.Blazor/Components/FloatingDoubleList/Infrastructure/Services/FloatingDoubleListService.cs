using Dnet.Blazor.Components.FloatingDoubleList.Infrastructure.Models;
using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.FloatingDoubleList.Infrastructure.Services;

public class FloatingDoubleListService<TItem>
{
    public event Action<TransferredItems<TItem>> OnSelectionChange;

    public event Action<SearchModel> OnSearchLeft;

    public event Action<SearchModel> OnSearchRight;

    public event Action<List<TItem>> OnRefreshDataLeft;

    public event Action<List<TItem>> OnRefreshDataRight;

    public void UpdateTransferredItems(TransferredItems<TItem> item)
    {
        OnSelectionChange?.Invoke(item);
    }

    public void SearchLeft(SearchModel searchModel)
    {
        OnSearchLeft?.Invoke(searchModel);
    }

    public void SearchRight(SearchModel searchModel)
    {
        OnSearchRight?.Invoke(searchModel);
    }

    public void RefreshDataLeft(List<TItem> items)
    {
        OnRefreshDataLeft?.Invoke(items);
    }

    public void RefreshDataRight(List<TItem> items)
    {
        OnRefreshDataRight?.Invoke(items);
    }
}
