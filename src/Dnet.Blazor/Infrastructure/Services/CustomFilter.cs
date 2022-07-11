using System.Linq.Expressions;

namespace Dnet.Blazor.Infrastructure.Services;

public class CustomFilter<TItem>
{
    private FilterPredicate<TItem> _firstExpression;

    private List<FilterPredicate<TItem>>? _thenExpressions;

    internal CustomFilter(Expression<Func<TItem, string>> firstExpression, CustomFilterOperator filterOperator)
    {
        _firstExpression = new FilterPredicate<TItem>
        {
            Predicate = firstExpression,
            CustomFilterOperator = filterOperator
        };
        _thenExpressions = default;
    }

    public static CustomFilter<TItem> FilterBy(Expression<Func<TItem, string>> expression, CustomFilterOperator filterOperator = CustomFilterOperator.Contains)
        => new CustomFilter<TItem>(expression, filterOperator);

    public CustomFilter<TItem> ThenFilterBy(Expression<Func<TItem, string>> expression, CustomFilterOperator filterOperator = CustomFilterOperator.Contains)
    {
        _thenExpressions ??= new();
        _thenExpressions.Add(new FilterPredicate<TItem>
        {
            Predicate = expression,
            CustomFilterOperator = filterOperator
        });
        return this;
    }

    internal IQueryable<TItem> Apply(IQueryable<TItem> queryable, string filterString)
    {
        var accessExpression = Expression.Parameter(typeof(TItem), "x");

        var mExpression = _firstExpression.Predicate.Body as MemberExpression;

        if (mExpression == null) return queryable;

        var name = mExpression.Member.Name;

        var type = typeof(TItem);

        var property = type.GetProperty(name);

        var propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

        var mainPredicate = GetOperationExpression(_firstExpression.CustomFilterOperator, filterString, propertyExpression);

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

                    var predicate = GetOperationExpression(expression.CustomFilterOperator, filterString, propertyExpression);

                    mainPredicate = Expression.Or(mainPredicate, predicate);
                }
            }
        }

        var queryExpresion = Expression.Lambda<Func<TItem, bool>>(mainPredicate, accessExpression);

        return queryable.Where(queryExpresion);
    }

    private Expression GetOperationExpression(CustomFilterOperator filterOperator, string value, Expression propertyExpression)
    {
        var wValue = value.ToUpper();

        var predicate = filterOperator switch
        {
            CustomFilterOperator.Contains => FilterByFunction(propertyExpression, "Contains", wValue),

            CustomFilterOperator.Equals => FilterByFunction(propertyExpression, "Equals", wValue),

            CustomFilterOperator.StartsWith => FilterByFunction(propertyExpression, "StartsWith", wValue),

            CustomFilterOperator.EndsWith => FilterByFunction(propertyExpression, "EndsWith", wValue),

            _ => throw new ArgumentOutOfRangeException(nameof(filterOperator), "value")
        };

        return predicate;
    }

    private static Expression FilterByFunction(Expression propertyExpression, string function, string value)
    {
        var filterMethod = typeof(string).GetMethods().FirstOrDefault(x => x.Name == function);

        if (filterMethod == null) throw new ArgumentNullException(nameof(propertyExpression));

        return Expression.Call(Expression.Call(propertyExpression, "ToUpper", null), filterMethod, Expression.Constant(value));
    }
}

internal class FilterPredicate<TItem>
{
    public Expression<Func<TItem, string>> Predicate { get; set; }

    public CustomFilterCondition OperatorType { get; set; }

    public CustomFilterOperator CustomFilterOperator { get; set; }
}
