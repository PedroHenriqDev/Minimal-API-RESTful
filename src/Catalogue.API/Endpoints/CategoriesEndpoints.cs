﻿using Catalogue.API.OpenApi;
using Catalogue.API.Filters;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Extensions;
using Catalogue.Application.Pagination;
using Catalogue.Application.Pagination.Parameters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        #region Get

        endpoints
        .MapGet("categories", GetCategoriesAsync)
        .Produces<PagedList<GetCategoryQueryResponse>>(StatusCodes.Status200OK)
        .WithGetCategoriesDoc();
      
        endpoints
        .MapGet("categories/{id:guid}", GetCategoryByIdAsync)
        .Produces<GetCategoryQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithName("GetCategoryById")
        .WithGetCategoryByIdDoc();
        
       
        endpoints
        .MapGet("categories/products", GetCategoriesWithProductsAsync)
        .Produces<PagedList<GetCategoryWithProdsQueryResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithGetCategoriesWithProductsDoc();

        endpoints
        .MapGet("categories/products/{id:Guid}", GetByIdCategoryWithProductsAsync)
        .Produces<GetCategoryWithProdsQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithName("GetByIdCategoryWithProducts")
        .WithGetCategoryIncludingProductsDoc();
        #endregion

        #region Post

        endpoints
        .MapPost("categories", CreateCategoryAsync)
        .Produces<CreateCategoryCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithPostCategoryDoc();
        
        endpoints
        .MapPost("categories/products", CreateCategoryWithProductsAsync)
        .Produces<CreateCategoryWithProdsCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithPostCategoryWithProductsDoc();
        #endregion

        #region Put

        endpoints
        .MapPut("categories/{id:Guid}", UpdateCategoryAsync)
        .AddEndpointFilter<InjectIdFilter>()
        .Produces<UpdateCategoryCommandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithPutCategoryDoc();

        #endregion

        #region Delete
        endpoints
        .MapDelete("categories/{id:Guid}", DeleteCategoryAsync)
        .Produces<DeleteCategoryCommandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithDeleteCategoryDoc();
        #endregion
    }

    private static async Task<IResult> GetCategoriesAsync
    (
        HttpContext httpContext,
        [AsParameters] QueryParameters parameters,
        [FromServices] IMediator mediator
    )
    {
        GetCategoriesQueryResponse response =
                        await mediator.Send(new GetCategoriesQueryRequest(parameters));

        httpContext.AppendCategoriesMetaData(response.CategoriesPaged);

        return Results.Ok(response.CategoriesPaged);
    }

    private static async Task<IResult> GetCategoryByIdAsync([FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        GetCategoryQueryResponse response = await mediator.Send(new GetCategoryQueryRequest(id));
        return Results.Ok(response);
    }
    
    private static async Task<IResult> GetCategoriesWithProductsAsync
    (   HttpContext httpContext,
        [AsParameters] QueryParameters parameters,
        [FromServices] IMediator mediator
    )
    {
        GetCategoriesWithProdsQueryResponse response =
                           await mediator.Send(new GetCategoriesWithProdsQueryRequest(parameters));

        httpContext.AppendCategoriesMetaData(response.CategoriesPaged);

        return Results.Ok(response.CategoriesPaged);
    }

    private static async Task<IResult> GetByIdCategoryWithProductsAsync
    (
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        GetCategoryWithProdsQueryResponse response =
                           await mediator.Send(new GetCategoryWithProdsQueryRequest(id));

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateCategoryAsync
    (
        [FromBody] CreateCategoryCommandRequest request,
        [FromServices] IMediator mediator
    )
    {
        CreateCategoryCommandResponse response = await mediator.Send(request);

        return Results.CreatedAtRoute
        (
            routeName: "GetCategoryById",
            routeValues: new { id = response.Id },
            value: response
        );
    }

    private static async Task<IResult> CreateCategoryWithProductsAsync
    (
        [FromBody] CreateCategoryWithProdsCommandRequest request,
        [FromServices] IMediator mediator
    ) 
    {
        CreateCategoryWithProdsCommandResponse response = await mediator.Send(request);
        return Results.CreatedAtRoute
        (
            routeName: "GetByIdCategoryWithProducts", 
            routeValues: new {id = response.Id}, value: response
        );
    }

    private static async Task<IResult> UpdateCategoryAsync
    (
        [FromBody] UpdateCategoryCommandRequest request,
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        UpdateCategoryCommandResponse response = await mediator.Send(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteCategoryAsync
    (
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
            DeleteCategoryCommandResponse response = await mediator.Send(new DeleteCategoryCommandRequest(id));
            return Results.Ok(response);
    }
}

