using Catalogue.API.Filters;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Interfaces;
using Catalogue.Application.Pagination;
using Catalogue.Application.Pagination.Parameters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Catalogue.API.Endpoints;

public static class CategoriesEndpoints
{
    const string endpointTag = "Categories";

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

    public static void MapGetCategoriesEndpoints(this WebApplication app)
    {
        app.MapGet("categories", async (HttpContext httpContext,
                                        [AsParameters] QueryParameters parameters,
                                        [FromServices] IMediator mediator) =>
        {
            GetCategoriesQueryResponse response = await mediator.Send
            (
                new GetCategoriesQueryRequest(parameters)
            );

            AppendCategoriesMetaData(httpContext, response.CategoriesPaged);

            return Results.Ok(response.CategoriesPaged);
        }).Produces<PagedList<GetCategoryQueryResponse>>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .WithTags(endpointTag);

        app.MapGet("categories/{id:int}", async ([FromRoute] int id, [FromServices] IMediator mediator) =>
        {
            GetCategoryQueryResponse response = await mediator.Send(new GetCategoryQueryRequest(id));

            return Results.Ok(response);
        }).Produces<GetCategoryQueryResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);

        app.MapGet("categories/products", async (HttpContext httpContext,
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
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .WithTags(endpointTag);

        app.MapGet("categories/{id:int}/products", async ([FromRoute] int id,
                                                          [FromServices] IMediator mediator) =>
        {
            GetCategoryWithProductsQueryResponse response = await mediator.Send
            (
                new GetCategoryWithProductsQueryRequest(id)
            );

            return Results.Ok(response);
        })
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);
    }

    public static void MapPostCategoriesEndpoints(this WebApplication app)
    {
        app.MapPost("categories", async ([FromBody] CreateCategoryCommandRequest request,
                                         [FromServices] IMediator mediator) =>
        {
            CreateCategoryCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);
        }).Produces<CreateCategoryCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .WithTags(endpointTag);

        app.MapPost("categories/products", async ([FromBody] CreateCategoryWithProdsCommandRequest request,
                                                  [FromServices] IMediator mediator) =>
        {

            CreateCategoryWithProdsCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);

        }).Produces<CreateCategoryWithProdsCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .WithTags(endpointTag);
    }

    public static void MapDeleteCategoriesEndpoints(this WebApplication app)
    {
        app.MapDelete("categories/{id:int}", async ([FromRoute] int id,
                                                    [FromServices] IMediator mediator) =>
        {
            DeleteCategoryCommandResponse response = await mediator.Send(new DeleteCategoryCommandRequest(id));
            return Results.Ok(response);
        }).Produces<DeleteCategoryCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);
    }

    public static void MapPutCategoriesEndpoints(this WebApplication app)
    {
        app.MapPut("categories/{id:int}", async ([FromBody] UpdateCategoryCommandRequest request,
                                                 [FromRoute] int id,
                                                 [FromServices] IMediator mediator) =>
        {
            UpdateCategoryCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);

        }).AddEndpointFilter<InjectIdFilter>()
          .Produces<UpdateCategoryCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);
    }
}
