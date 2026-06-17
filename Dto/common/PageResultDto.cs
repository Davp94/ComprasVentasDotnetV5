using System;

namespace ComprasVentas;

public class PageResultDto<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public int TotalCount { get; set; }

    public int Page { get; set; }

    public int Size { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount/Size);

    public static PageResultDto<T> Build(IEnumerable<T> items, int totalCount, int page, int size)
    {
        return new PageResultDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            Size = size
        };
    }

}
