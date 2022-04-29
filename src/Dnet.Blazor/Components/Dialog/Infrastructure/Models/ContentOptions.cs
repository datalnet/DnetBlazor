namespace Dnet.Blazor.Components.Dialog.Infrastructure.Models
{
    public class ContentOptions<TItem>
    {
        public int OverlayReferenceId { get; set; }

        public TItem Options { get; set; }
    }
}
