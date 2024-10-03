using Catalogue.API.Filters;
using Catalogue.API.OpenApi;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Extensions;
using Catalogue.Application.Pagination;
using Catalogue.Application.Pagination.Parameters;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using Catalogue.Application.Products.Queries.Requests;
using Catalogue.Application.Products.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        #region Get
        endpoints
        .MapGet("products", GetAllAsync)
        .Produces<PagedList<GetProductQueryResponse>>(StatusCodes.Status200OK)
        .RequireAuthorization()
        .WithGetProductsDoc();

        endpoints
        .MapGet("products/{id:Guid}", GetByIdAsync)
        .Produces<GetProductQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithName("GetProductById")
        .WithGetByIdProductDoc();

        endpoints
        .MapGet("products/category", GetAllWithCategoryAsync )
        .Produces<PagedList<GetProductWithCatQueryResponse>>(StatusCodes.Status200OK)
        .RequireAuthorization()
        .WithGetProductsWithCategoryDoc();

        endpoints
        .MapGet("products/{id:Guid}/category", GetByIdWithCategoryAsync)
        .Produces<GetProductWithCatQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithGetByIdProductWithCategoryDoc();
        
        #endregion

        #region Post

        endpoints
        .MapPost("products", CreateAsync)
        .Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithPostProductDoc();

        endpoints
        .MapPost("products/category-name", CreateByCategoryNameAsync)
        .Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithPostProductByCategoryNameDoc();

        #endregion

        #region Put

        endpoints
        .MapPut("products/{id:Guid}", UpdateAsync)
        .AddEndpointFilter<InjectIdFilter>()
        .Produces<UpdateProductCommandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithPutProductDoc();

        endpoints.MapDelete("products/{id:Guid}", DeleteAsync)
        .Produces<DeleteProductCommandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithDeleteProductDoc();

        #endregion
    }

    private static async Task<IResult> GetAllAsync
    (
        HttpContext httpContext,
        [AsParameters] QueryParameters parameters,
        [FromServices] IMediator mediator
    ) 
    {
        GetProductsQueryResponse response =
        await mediator.Send(new GetProductsQueryRequest(parameters));
        
        httpContext.AppendCategoriesMetaData(response.ProductsPaged);

        return Results.Ok(response.ProductsPaged);
    }    

    private static async Task<IResult> GetByIdAsync([FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        GetProductQueryResponse response = await mediator.Send(new GetProductQueryRequest(id));
        return Results.Ok(response);
    }

    private static async Task<IResult> GetAllWithCategoryAsync
    (   
        HttpContext httpContext,
        [AsParameters] QueryParameters parameters,
        [FromServices] IMediator mediator) 
    {
        GetProductsWithCatQueryResponse response =
        await mediator.Send(new GetProductsWithCatQueryRequest(parameters));
        httpContext.AppendCategoriesMetaData(response.ProductsPaged);

        return Results.Ok(response.ProductsPaged);
    }

    private static async Task<IResult> GetByIdWithCategoryAsync
    (
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )  
    {
        GetProductWithCatQueryResponse response = await mediator.Send(new GetProductWithCatQueryRequest(id));
        return Results.Ok(response);
    }

    private static async Task<IResult> CreateAsync
    (
        [FromBody] CreateProductCommandRequest request, 
        [FromServices] IMediator mediator
    )
    {
        CreateProductCommandResponse response = await mediator.Send(request);

        return Results.CreatedAtRoute(
        routeName: "GetProductById",
        routeValues: new {id = response.Id},
        value: response);
    }

    private static async Task<IResult> CreateByCategoryNameAsync 
    (
        [FromBody] CreateProductByCatNameCommandRequest request,
        [FromServices] IMediator mediator
    ) 
    {
        CreateProductCommandResponse response = await mediator.Send(request);

        return Results.CreatedAtRoute(
        routeName: "GetProductById", 
        routeValues: new { id = response.Id },
        value: response);
    }

    private static async Task<IResult> UpdateAsync
    (
        [FromBody] UpdateProductCommandRequest request,
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    ) 
    {
        UpdateProductCommandResponse response = await mediator.Send(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteAsync
    (
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        DeleteProductCommandResponse response = await mediator.Send(new DeleteProductCommandRequest(id));
        return Results.Ok(response);
    }
}
