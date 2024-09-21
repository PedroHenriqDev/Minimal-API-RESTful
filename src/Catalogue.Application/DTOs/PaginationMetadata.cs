namespace Catalogue.Application.DTOs;

public class PaginationMetadata
{
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public int PageCurrent {get; set;}
    public bool HasPreviousPage {  get; set; }
    public bool HasNextPage { get; set; }
    public int ItemsCount { get; set; }
}
