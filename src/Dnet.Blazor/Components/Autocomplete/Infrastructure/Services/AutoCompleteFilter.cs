using System.Linq.Expressions;

namespace Dnet.Blazor.Components.Autocomplete.Infrastructure.Services;

public class AutoCompleteFilter<TItem>
{
    private Expression<Func<TItem, string>> _firstExpression;

    private List<FilterPredicate<TItem>>? _thenExpressions;

    internal AutoCompleteFilter(Expression<Func<TItem, string>> firstExpression)
    {
        _firstExpression = firstExpression;
        _thenExpressions = default;
    }

    public static AutoCompleteFilter<TItem> FilterBy(Expression<Func<TItem, string>> expression)
        => new AutoCompleteFilter<TItem>(expression);

    public AutoCompleteFilter<TItem> ThenFilterBy(Expression<Func<TItem, string>> expression)
    {
        _thenExpressions ??= new();
        _thenExpressions.Add(new FilterPredicate<TItem>
        {
            Predicate = expression,
        });
        return this;
    }

    internal IQueryable<TItem> Apply(IQueryable<TItem> queryable, string filterString)
    {
        var accessExpression = Expression.Parameter(typeof(TItem), "x");

        var mExpression = _firstExpression.Body as MemberExpression;

        if (mExpression == null) return queryable;

        var name = mExpression.Member.Name;

        var type = typeof(TItem);

        var property = type.GetProperty(name);

        var propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

        var mainPredicate = GetOperationExpression(FilterOperator.Contains, filterString, propertyExpression);

        if (_thenExpressions is not null)
        {
            foreach (var expression in _thenExpressions)
            {
                mExpression = expression.Predicate.Body as MemberExpression;

                if (mExpression != null)
                {
                    name = mExpression.Member.Name;

                    type = typeof(TItem);

                    property = type.GetProperty(name);

                    propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

                    var predicate = GetOperationExpression(FilterOperator.Contains, filterString, propertyExpression);

                    mainPredicate = Expression.Or(mainPredicate, predicate);
                }
            }
        }

        var queryExpresion = Expression.Lambda<Func<TItem, bool>>(mainPredicate, accessExpression);

        return queryable.Where(queryExpresion);
    }

    private Expression GetOperationExpression(FilterOperator filterOperator, string value, Expression propertyExpression)
    {
        var wValue = value;

        var predicate = filterOperator switch
        {
            FilterOperator.Contains => FilterByFunction(propertyExpression, "Contains", wValue),

            FilterOperator.NotContains => FilterByFunctionNegation(propertyExpression, "Contains", wValue),

            FilterOperator.Equals => FilterByFunction(propertyExpression, "Equals", wValue),

            FilterOperator.NotEquals => FilterByFunctionNegation(propertyExpression, "Equals", wValue),

            FilterOperator.StartsWith => FilterByFunction(propertyExpression, "StartsWith", wValue),

            FilterOperator.EndsWith => FilterByFunction(propertyExpression, "EndsWith", wValue),

            _ => throw new ArgumentOutOfRangeException(nameof(filterOperator), "value")
        };

        return predicate;
    }

    private static Expression FilterByFunction(Expression propertyExpression, string function, string value)
    {
        var filterMethod = typeof(string).GetMethods().FirstOrDefault(x => x.Name == function);

        if (filterMethod == null) throw new ArgumentNullException(nameof(propertyExpression));

        return Expression.Call(propertyExpression, filterMethod, Expression.Constant(value));
    }

    private static Expression FilterByFunctionNegation(Expression propertyExpression, string function, string value)
    {
        var filterMethod = typeof(string).GetMethods().FirstOrDefault(x => x.Name == function);

        if (filterMethod == null) throw new ArgumentNullException(nameof(filterMethod));

        return Expression.Not(Expression.Call(propertyExpression, filterMethod, Expression.Constant(value)));
    }
}

internal class FilterPredicate<TItem>
{
    public Expression<Func<TItem, string>> Predicate { get; set; }

    public FilterCondition OperatorType { get; set; }

    public FilterOperator FilterOperator { get; set; }
}
