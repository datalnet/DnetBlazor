namespace Dnet.Blazor.Components.Select.Infrastructure.Services
{
    public class SelectService<TItem>
    {
        public event Action<List<TItem>>? OnUpdateList;

        public event Action<TItem>? OnItemSelected;

        public event Action<List<RowNode<TItem>>>? OnSelectionChange;

        public void UdateList(List<TItem> items)
        {
            OnUpdateList?.Invoke(items);
        }

        public void UpdateSelectedItem(TItem item)
        {
            OnItemSelected?.Invoke(item);
        }
        public void UpdateSelectedItems(List<RowNode<TItem>> items)
        {
            OnSelectionChange?.Invoke(items);
        }

    }
}
