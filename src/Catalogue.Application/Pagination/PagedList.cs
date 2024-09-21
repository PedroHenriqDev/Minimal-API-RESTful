using Catalogue.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Application.Pagination;

public class PagedList<T> : List<T>, IPagedList<T>
{
    private const int MAX_PAGE_SIZE = 50;

    private int pageSize { get; set; }

    public int PageCurrent { get; set; } = 1;
    public int PageCount { get; set; }
    public int ItemsCount { get; set; }

    public bool HasPreviousPage => PageCurrent > 1;
    public bool HasNextPage => PageCurrent < PageCount;
    public int PageSize => (pageSize > 0 && pageSize <= MAX_PAGE_SIZE) ? pageSize : MAX_PAGE_SIZE; 

    public PagedList()
    {
    }

    public PagedList(int pageSize, int pageNumber, IEnumerable<T> source)
    {
        this.pageSize = pageSize;
        PageCurrent = pageNumber;
        PageCount = (int)Math.Ceiling(source.Count() / (double)pageSize);
        ItemsCount = source.Count();

        IEnumerable<T> items = source.Skip((PageCurrent - 1) * pageSize).Take(pageSize);

        AddRange(items);
    }

    public static async Task<PagedList<T>> ToPagedListAsync(int pageNumber, int pageSize, IQueryable<T> source) 
    {
        return new PagedList<T>(pageSize < 1 ? MAX_PAGE_SIZE : pageSize,
                                pageNumber < 1 ? 1 : pageNumber,
                                await source.ToListAsync());
    }

    public static PagedList<T> ToPagedList(int pageNumber, int pageSize, IQueryable<T> source)
    {
        return new PagedList<T>(pageSize < 1 ? MAX_PAGE_SIZE : pageSize,
                                pageNumber < 1 ? 1 : pageNumber,
                                source.ToList());
    }
}
