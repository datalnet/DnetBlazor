using Dnet.Blazor.Infrastructure.Models.SearchModels;

namespace Dnet.Blazor.Components.List
{
    public class ListOptions<TItem>
    {
        public Func<TItem, string>? DisplayValueConverter { get; set; }

        public Func<TItem, string>? SupportTextValueConverter { get; set; }

        public Func<TItem, string>? SortedData { get; set; }

        public Func<TItem, string>? SearchValueConverter { get; set; }

        public Func<TItem, bool>? DisableItem { get; set; }

        public string? HeaderText { get; set; }

        public string? SearchInputPlaceHolder { get; set; }

        public string? SearchInputLabel { get; set; }

        public float ItemSize { get; set; } = 40f;

        public bool MultiSelect { get; set; } = true;

        public double DebounceTime { get; set; } = 250;

        public bool EnableServerSide { get; set; }

        public bool EnableSearching { get; set; }

        public bool EnableSorting { get; set; }

        public int PageSize { get; set; } = 100;

        public bool EnablePagingination { get; set; }

        public bool CheckboxSelectionColumn { get; set; }

        public bool ShowHeader { get; set; }

        public bool ShowFooter { get; set; }

        public string? ContainerName { get; set; }

        public string? ConnectedTo { get; set; }

        public string? ServerSideSortColumn { get; set; }

        public List<string>? ServerSideSearchColumns { get; set; }

        public SortOrder ServerSideSortOrder { get; set; } = SortOrder.Ascending;
    }
}
