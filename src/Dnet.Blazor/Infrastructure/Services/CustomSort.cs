using System.Linq.Expressions;

namespace Dnet.Blazor.Infrastructure.Services;

public class CustomSort<TItem>
{
    private SortPredicate<TItem> _firstExpression;

    private List<SortPredicate<TItem>>? _thenExpressions;

    internal CustomSort(Expression<Func<TItem, string>> firstExpression, CustomSortOrder sortOrder)
    {
        _firstExpression = new SortPredicate<TItem>
        {
            Predicate = firstExpression,
            SortOrder = sortOrder
        };

        _thenExpressions = default;
    }

    public static CustomSort<TItem> ByAscending<U>(Expression<Func<TItem, string>> expression)
        => new CustomSort<TItem>(expression, CustomSortOrder.Ascending);

    public static CustomSort<TItem> ByDescending<U>(Expression<Func<TItem, string>> expression)
        => new CustomSort<TItem>(expression, CustomSortOrder.Descending);

    public CustomSort<TItem> ThenAscending<U>(Expression<Func<TItem, string>> expression)
    {
        _thenExpressions ??= new();
        _thenExpressions.Add(new SortPredicate<TItem>
        {
            Predicate = expression,
            SortOrder = CustomSortOrder.Ascending
        });
        return this;
    }

    public CustomSort<TItem> ThenDescending<U>(Expression<Func<TItem, string>> expression)
    {
        _thenExpressions ??= new();
        _thenExpressions.Add(new SortPredicate<TItem>
        {
            Predicate = expression,
            SortOrder = CustomSortOrder.Descending
        });
        return this;
    }

    internal IQueryable<TItem> Apply(IQueryable<TItem> queryable, string filterString, bool orderStartingBySearchedString)
    {
        //E1 = x 
        var accessExpression = Expression.Parameter(typeof(TItem), "x");

        var mExpression = _firstExpression.Predicate.Body as MemberExpression;

        if (mExpression == null) return queryable;

        var name = mExpression.Member.Name;

        var type = typeof(TItem);

        var property = type.GetProperty(name);

        //E1.PROPERTY
        var propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

        //E1 => E1.PROPERTY
        var expression3 = Expression.Lambda(propertyExpression, accessExpression);

        var toUpperfilterString = filterString.ToUpper();

        if(orderStartingBySearchedString)
        {
            var filterMethod = typeof(string).GetMethods().FirstOrDefault(x => x.Name == "StartsWith");

            //E1.PROPERTY.ToUpper().StartsWith(toUpperfilterString)
            var startsWithExp = Expression.Call(Expression.Call(propertyExpression, "ToUpper", null), filterMethod, Expression.Constant(toUpperfilterString));

            //E1 => E1.PROPERTY.ToUpper().StartsWith(toUpperfilterString)
            Expression<Func<TItem, bool>> orderByExpression = Expression.Lambda<Func<TItem, bool>>(startsWithExp, accessExpression);

            queryable = queryable.OrderByDescending(orderByExpression);
            
            var sortType = _firstExpression.SortOrder == CustomSortOrder.Ascending ? "ThenBy" : "ThenByDescending";

            var thenByExp = Expression.Call(typeof(Queryable), sortType, new[] { type, property.PropertyType }, queryable.Expression, expression3);

            queryable = queryable.Provider.CreateQuery<TItem>(thenByExp);
        }
        else
        {
            var sortType = _firstExpression.SortOrder == CustomSortOrder.Ascending ? "OrderBy" : "OrderByDescending";

            var orderBy = Expression.Call(typeof(Queryable), sortType, new[] { type, property.PropertyType }, queryable.Expression, expression3);

            queryable = queryable.Provider.CreateQuery<TItem>(orderBy);
        }

        if (_thenExpressions is not null)
        {
            foreach (var thenExpression in _thenExpressions)
            {
                mExpression = thenExpression.Predicate.Body as MemberExpression;

                if (mExpression != null)
                {
                    name = mExpression.Member.Name;

                    type = typeof(TItem);

                    property = type.GetProperty(name);

                    propertyExpression = Expression.MakeMemberAccess(accessExpression, property);

                    var expression4 = Expression.Lambda(propertyExpression, accessExpression);

                    var sortType1 = thenExpression.SortOrder == CustomSortOrder.Ascending ? "ThenBy" : "ThenByDescending";

                    var thenByExp1 = Expression.Call(typeof(Queryable), sortType1, new[] { type, property.PropertyType }, queryable.Expression, expression4);

                    queryable = queryable.Provider.CreateQuery<TItem>(thenByExp1);
                }
            }
        }

        return queryable;
    }
}

internal class SortPredicate<TItem>
{
    public Expression<Func<TItem, string>> Predicate { get; set; }

    public CustomSortOrder SortOrder { get; set; }
}
