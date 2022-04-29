namespace Dnet.Blazor.Components.Paginator;

public interface IPaginator
{
    Tuple<int, int> GetRangePage(int currentPage, int pageSize, int totalItems);

    string GetRangeLabel(int page, int pageSize, int length);
}
