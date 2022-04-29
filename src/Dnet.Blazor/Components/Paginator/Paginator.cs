namespace Dnet.Blazor.Components.Paginator;

public class Paginator : IPaginator
{
    public Tuple<int, int> GetRangePage(int currentPage, int pageSize, int totalItems)
    {
        return new Tuple<int, int>((currentPage - 1) * pageSize, Math.Min(pageSize, totalItems - (currentPage - 1) * pageSize));
    }

    public string GetRangeLabel(int page, int pageSize, int length)
    {
        if (length == 0 || pageSize == 0) { return $"0 of {length}"; }

        length = Math.Max(length, 0);

        var startIndex = (page - 1) * pageSize;

        // If the start index exceeds the list length, do not try and fix the end index to the end.
        var endIndex = startIndex < length ? Math.Min(startIndex + pageSize, length) : startIndex + pageSize;

        return $"{startIndex + 1} - {endIndex} of { length}";
    }
}
