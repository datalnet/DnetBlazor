using System.Linq.Expressions;

namespace Dnet.Blazor.Infrastructure.Services;

public class CustomSortOld<TItem>
{
    private Func<IQueryable<TItem>, IOrderedQueryable<TItem>> _first;

    private List<Func<IOrderedQueryable<TItem>, IOrderedQueryable<TItem>>>? _then;

    internal CustomSortOld(Func<IQueryable<TItem>, IOrderedQueryable<TItem>> first)
    {
        _first = first;
        _then = default;
    }

    public static CustomSortOld<TItem> ByAscending<U>(Expression<Func<TItem, U>> expression)
        => new CustomSortOld<TItem>((queryable) => queryable.OrderBy(expression));

    public static CustomSortOld<TItem> ByDescending<U>(Expression<Func<TItem, U>> expression)
        => new CustomSortOld<TItem>((queryable) => queryable.OrderByDescending(expression));

    public CustomSortOld<TItem> ThenAscending<U>(Expression<Func<TItem, U>> expression)
    {
        _then ??= new();
        _then.Add((queryable) => queryable.ThenBy(expression));
        return this;
    }

    public CustomSortOld<TItem> ThenDescending<U>(Expression<Func<TItem, U>> expression)
    {
        _then ??= new();
        _then.Add((queryable) => queryable.ThenByDescending(expression));
        return this;
    }

    internal IOrderedQueryable<TItem> Apply(IQueryable<TItem> queryable)
    {
        var orderedQueryable = _first(queryable);

        if (_then is not null)
        {
            foreach (var clause in _then)
            {
                orderedQueryable = clause(orderedQueryable);
            }
        }

        return orderedQueryable;
    }
}
