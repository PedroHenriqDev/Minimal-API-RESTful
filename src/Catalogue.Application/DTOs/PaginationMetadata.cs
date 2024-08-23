namespace Catalogue.Application.DTOs;

public class PaginationMetadata
{
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public bool HasPrevious {  get; set; }
    public bool HasNext { get; set; }
    public int  TotalItems { get; set; }
}
