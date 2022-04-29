namespace Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;

public class AdvancedFilterModel
{
    public string? Column { get; set; }

    public string? Value { get; set; }

    public string? AdditionalValue { get; set; }

    public CellDataType Type { get; set; }

    public FilterOperator Operator { get; set; }

    public FilterOperator AdditionalOperator { get; set; }

    public FilterCondition Condition { get; set; }

    public string? DateFormat { get; set; }

    public FilterCondition FilterLinkCondition { get; set; } = FilterCondition.And;
}

