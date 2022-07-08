using System.Linq.Expressions;

namespace Dnet.Blazor.Infrastructure.Services;

public class CustomSort<TItem>
{
    private Func<IQueryable<TItem>, IOrderedQueryable<TItem>> _first;

    private List<Func<IOrderedQueryable<TItem>, IOrderedQueryable<TItem>>>? _then;

    internal CustomSort(Func<IQueryable<TItem>, IOrderedQueryable<TItem>> first)
    {
        _first = first;
        _then = default;
    }

    public static CustomSort<TItem> ByAscending<U>(Expression<Func<TItem, U>> expression)
        => new CustomSort<TItem>((queryable) => queryable.OrderBy(expression));

    public static CustomSort<TItem> ByDescending<U>(Expression<Func<TItem, U>> expression)
        => new CustomSort<TItem>((queryable) => queryable.OrderByDescending(expression));

    public CustomSort<TItem> ThenAscending<U>(Expression<Func<TItem, U>> expression)
    {
        _then ??= new();
        _then.Add((queryable) => queryable.ThenBy(expression));
        return this;
    }

    public CustomSort<TItem> ThenDescending<U>(Expression<Func<TItem, U>> expression)
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
