using Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;

namespace Dnet.Blazor.Infrastructure.Models.SearchModels;

public class SearchModel
{
    public PaginationModel PaginationModel { get; set; }

    public List<AdvancedFilterModel> AdvancedFilterModels { get; set; }

    public List<FilterModel> FilterModels { get; set; }

    public SortModel SortModel { get; set; }
}
