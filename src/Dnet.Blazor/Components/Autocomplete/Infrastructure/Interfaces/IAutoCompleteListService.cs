namespace Dnet.Blazor.Components.Autocomplete.Infrastructure.Interfaces
{
    public interface IAutoCompleteListService<TItem>
    {
        event Action<List<TItem>> OnUpdateList;

        void UdateList(List<TItem> items);
    }
}
