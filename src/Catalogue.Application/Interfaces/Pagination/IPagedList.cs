namespace Catalogue.Application.Interfaces;

public interface IPagedList<T> : IEnumerable<T>
{
    int PageCurrent { get; set;  }
    int PageCount { get; set; }
    int ItemsCount { get; set; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get;  }
    int PageSize { get; }
}
