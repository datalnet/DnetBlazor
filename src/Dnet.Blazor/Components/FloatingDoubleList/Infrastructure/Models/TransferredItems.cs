namespace Dnet.Blazor.Components.FloatingDoubleList.Infrastructure.Models;

public class TransferredItems<TItem>
{
    public List<TItem> RightAddedItems = new();

    public List<TItem> RightRemovedItems = new();

    public List<TItem> LeftAddedItems = new();

    public List<TItem> LeftRemovedItems = new();
}
