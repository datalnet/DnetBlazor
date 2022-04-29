namespace Dnet.Blazor.Components.Autocomplete.Infrastructure.Models
{
    public class AutoCompleteData<TItem>
    {
        public int AutoCompleteDataId { get; set; }

        public TItem Item { get; set; }

    }
}
