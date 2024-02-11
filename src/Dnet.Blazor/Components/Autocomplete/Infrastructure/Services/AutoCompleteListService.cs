namespace Dnet.Blazor.Components.Autocomplete.Infrastructure.Services
{
    public class AutoCompleteListService<TItem>
    {
        public event Action<List<TItem>>? OnUpdateList;

        public event Action<TItem>? OnItemSelected;

        public event Action<string>? OnUpdateCurrentValueFrontDialog;

        public void UdateList(List<TItem> items)
        {
            OnUpdateList?.Invoke(items);
        }

        public void UpdateSelectedItem(TItem item)
        {
            OnItemSelected?.Invoke(item);
        }

        public void UpdateCurrentValue(string currentValue)
        {
            OnUpdateCurrentValueFrontDialog?.Invoke(currentValue);
        }


    }
}
