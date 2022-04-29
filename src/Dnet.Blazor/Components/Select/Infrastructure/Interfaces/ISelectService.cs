namespace Dnet.Blazor.Components.Select.Infrastructure.Interfaces
{
    public interface ISelectService<TItem>
    {
        event Action<List<TItem>> OnUpdateList;

        void UdateList(List<TItem> items);
    }
}
