namespace Dnet.Blazor.Components.List
{
    public class DnetListDragAndDropService<TItem>
    {
        public List<TItem> TransferData { get; set; }

        public string FromContainer { get; set; }

        public void StartDrag(List<TItem> transferData, string fromContainer)
        {
            TransferData = transferData;
            FromContainer = fromContainer;
        }
    }
}
