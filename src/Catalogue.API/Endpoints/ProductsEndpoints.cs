using Catalogue.API.Filters;
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
    private const string productsTag = "Products";

    public static void MapGetProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("products", async (HttpContext httpContext,
                                      [AsParameters] QueryParameters parameters,
                                      [FromServices] IMediator mediator) =>
        {
            GetProductsQueryResponse response =
                        await mediator.Send(new GetProductsQueryRequest(parameters));
            httpContext.AppendCategoriesMetaData(response.ProductsPaged);

            return Results.Ok(response.ProductsPaged);
        }).Produces<PagedList<GetProductQueryResponse>>(StatusCodes.Status200OK)
          .RequireAuthorization()
          .WithTags(productsTag);

        endpoints.MapGet("products/{id:Guid}", async ([FromRoute] Guid id,
                                               [FromServices] IMediator mediator) =>
        {
            GetProductQueryResponse response = await mediator.Send(new GetProductQueryRequest(id));
            return Results.Ok(response);

        }).Produces<GetProductQueryResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .RequireAuthorization()
          .WithTags(productsTag);

        endpoints.MapGet("products/category", async (HttpContext httpContext,
                                               [AsParameters] QueryParameters parameters,
                                               [FromServices] IMediator mediator) =>
        {
            GetProductsWithCatQueryResponse response =
                        await mediator.Send(new GetProductsWithCatQueryRequest(parameters));
            httpContext.AppendCategoriesMetaData(response.ProductsPaged);

            return Results.Ok(response.ProductsPaged);

        }).Produces<PagedList<GetProductWithCatQueryResponse>>(StatusCodes.Status200OK)
          .RequireAuthorization()
          .WithTags(productsTag);

        endpoints.MapGet("products/{id:Guid}/category", async ([FromRoute] Guid id,
                                                        [FromServices] IMediator mediator) => 
        {
            GetProductWithCatQueryResponse response = await mediator.Send(new GetProductWithCatQueryRequest(id));
            return Results.Ok(response);

        }).Produces<GetProductWithCatQueryResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .RequireAuthorization()
          .WithTags(productsTag);
    }

    public static void MapPostProductsEndpoints(this IEndpointRouteBuilder endpoints) 
    {
        endpoints.MapPost("products", async ([FromBody] CreateProductCommandRequest request, 
                                       [FromServices] IMediator mediator) =>
        {
            CreateProductCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);

        }).Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .RequireAuthorization()
          .WithTags(productsTag);

        endpoints.MapPost("products/category-name", async ([FromBody] CreateProductByCatNameCommandRequest request,
                                                     [FromServices] IMediator mediator) =>
        {
            CreateProductCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);

        }).Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .RequireAuthorization()
          .WithTags(productsTag);
    }

    public static void MapPutProductsEndpoints(this IEndpointRouteBuilder endpoints) 
    {
        endpoints.MapPut("products/{id:Guid}", async ([FromBody] UpdateProductCommandRequest request,
                                               [FromRoute] Guid id,
                                               [FromServices] IMediator mediator) =>
        {
            UpdateProductCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);

        })
          .AddEndpointFilter<InjectIdFilter>()
          .Produces<UpdateProductCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .RequireAuthorization()
          .WithTags(productsTag);
    }

    public static void MapDeleteProductsEndpoints(this IEndpointRouteBuilder endpoints) 
    {
        endpoints.MapDelete("products/{id:Guid}", async ([FromRoute] Guid id,
                                                  [FromServices] IMediator mediator) =>
        {
            DeleteProductCommandResponse response = await mediator.Send(new DeleteProductCommandRequest(id));
            return Results.Ok(response);

        }).Produces<DeleteProductCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .RequireAuthorization()
          .WithTags(productsTag);
    }
}
