namespace Dnet.Blazor.Infrastructure.Models.SearchModels.FilterModels;

public enum FilterOperator
{
    None = 0,
    Equals = 1,
    NotEquals = 2,
    StartsWith = 3,
    EndsWith = 4,
    Contains = 5,
    NotContains = 6,
    GreaterThan = 7,
    LessThan = 8,
    Range = 9,
    GreaterThanOrEqual = 10,
    LessThanOrEqual = 11
}
