using Catalogue.Application.DTOs;
using Catalogue.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Catalogue.Application.Extensions;
 
public static class HttpContextExtension
{
    public static void AppendCategoriesMetaData<T>(this HttpContext httpContext, IPagedList<T>? categoriesPaged)
    {
        var metaData = new PaginationMetadata
        {
            PageSize = categoriesPaged?.PageSize ?? 0,
            PageCount = categoriesPaged?.PageCount ?? 0,
            HasPrevious = categoriesPaged?.HasPreviousPage ?? false,
            HasNext = categoriesPaged?.HasNextPage ?? false,
            TotalItems = categoriesPaged?.ItemsCount ?? 0
        };

        httpContext.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
    }

}
