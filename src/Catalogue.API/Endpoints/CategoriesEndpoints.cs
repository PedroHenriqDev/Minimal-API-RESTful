using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs;
using Catalogue.Application.Interfaces;
using Catalogue.Application.Pagination;
using Catalogue.Application.Pagination.Parameters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Catalogue.API.Endpoints;

public static class CategoriesEndpoints
{
    private static void AppendCategoriesMetaData<T>(HttpContext httpContext, IPagedList<T>? categoriesPaged)
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

    public static void MapGetCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("Categories", async (HttpContext httpContext,
                                        [AsParameters] QueryParameters parameters,
                                        [FromServices] IMediator mediator) =>
        {
            GetCategoriesQueryResponse response = await mediator.Send(new GetCategoriesQueryRequest(parameters));

            AppendCategoriesMetaData(httpContext, response.CategoriesPaged);

            return Results.Ok(response.CategoriesPaged);
        }).Produces<PagedList<GetCategoryQueryResponse>>(StatusCodes.Status200OK)
          .Produces<ErrorsDto>(StatusCodes.Status400BadRequest);

        app.MapGet("Categories/Products", async (HttpContext httpContext,
                                                 [AsParameters] QueryParameters parameters,
                                                 [FromServices] IMediator mediator) =>
        {
            GetCategoriesWithProductsQueryResponse response = await mediator.Send
            (
                new GetCategoriesWithProductsQueryRequest(parameters)
            );

            AppendCategoriesMetaData(httpContext, response.CategoriesPaged);

            return Results.Ok(response.CategoriesPaged);
        }).Produces<PagedList<GetCategoryWithProductsQueryResponse>>(StatusCodes.Status200OK)
          .Produces<ErrorsDto>(StatusCodes.Status400BadRequest);


        app.MapGet("Categories/{id:int}", async ([FromRoute] int id, [FromServices] IMediator mediator) =>
        {
            GetCategoryQueryResponse response = await mediator.Send(new GetCategoryQueryRequest(id));

            return Results.Ok(response);
        }).Produces<GetCategoryQueryResponse>(StatusCodes.Status200OK)
          .Produces<ErrorsDto>(StatusCodes.Status404NotFound);
    }

    public static void MapPostCategoryEndpoints(this WebApplication app)
    {
        app.MapPost("Categories", async ([FromBody] CreateCategoryCommandRequest request,
                                         [FromServices] IMediator mediator) =>
        {
            CreateCategoryCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);
        }).Produces<CreateCategoryCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorsDto>(StatusCodes.Status500InternalServerError);
    }

    public static void MapDeleteCategoryEndpoints(this WebApplication app)
    {
        app.MapDelete("Categories/{id:int}", async ([FromRoute] int id,
                                                    [FromServices] IMediator mediator) =>
        {
            var request = new DeleteCategoryCommandRequest { Id = id };
            DeleteCategoryCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);
        }).Produces<DeleteCategoryCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorsDto>(StatusCodes.Status404NotFound);
    }

    public static void MapUpdateCategoryEndpoints(this WebApplication app)
    {
        app.MapPut("Categories/{id:int}", async ([FromRoute] int id,
                                                 [FromBody] UpdateCategoryCommandRequest request,
                                                 [FromServices] IMediator mediator) =>
        {
            request.Id = id;
            UpdateCategoryCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);
        }).Produces<UpdateCategoryCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorsDto>(StatusCodes.Status400BadRequest)
          .Produces<ErrorsDto>(StatusCodes.Status404NotFound);
    }
}
