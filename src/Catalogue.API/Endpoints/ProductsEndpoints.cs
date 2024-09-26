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
    private const string productsTag = "Products";
  
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        #region Get
        /// <summary>
        /// Get a list products with pagination.
        /// </summary>
        /// <param name="httpContext">The current HTTP context used to access request and response metadata,
        /// including headers.</param>
        /// <param name="parameters">An object containing the query parameters for pagination, sorting, and
        /// filtering the list of products.</param>
        /// <param name="mediator">The MediatR instance responsible for sending the query to get a paginated
        /// list of products.</param>
        /// <returns>A paginated list of products.</returns>
        /// <response code="200">Returns a 200 OK status along with the paginated list of products.</response>
        /// <response code="401">Returns a 401 Unauthorized status if the user is not authenticated.</response>
        /// <response code="403">Returns a 403 Forbidden status if the user does not have the necessary
        /// permissions.</response>
         endpoints.MapGet("products", async 
         (  HttpContext httpContext,
            [AsParameters] QueryParameters parameters,
            [FromServices] IMediator mediator
         ) =>
         {
            GetProductsQueryResponse response =
            await mediator.Send(new GetProductsQueryRequest(parameters));

            httpContext.AppendCategoriesMetaData(response.ProductsPaged);

            return Results.Ok(response.ProductsPaged);
         })
        .Produces<PagedList<GetProductQueryResponse>>(StatusCodes.Status200OK)
        .RequireAuthorization()
        .WithGetProductsDoc();

        /// <summary>
        /// Retrieves a product by its unique identifier (GUID).
        /// </summary>
        /// <param name="id">The GUID of the product to retrieve.</param>
        /// <param name="mediator">The MediatR instance responsible for handling the request to get
        /// the product.</param>
        /// <returns>The product details if found; otherwise, a 404 Not Found response.</returns>
        /// <response code="200">Returns the product details when found.</response>
        /// <response code="404">Returns a 404 Not Found error if the product does not exist.</response>
        endpoints.MapGet("products/{id:Guid}", async ([FromRoute] Guid id,
                                               [FromServices] IMediator mediator) =>
        {
            GetProductQueryResponse response = await mediator.Send(new GetProductQueryRequest(id));
            return Results.Ok(response);

        })
        .Produces<GetProductQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithName("GetProductById")
        .WithGetByIdProductDoc();

        endpoints.MapGet("products/category", async (HttpContext httpContext,
                                               [AsParameters] QueryParameters parameters,
                                               [FromServices] IMediator mediator) =>
        {
            GetProductsWithCatQueryResponse response =
                        await mediator.Send(new GetProductsWithCatQueryRequest(parameters));
            httpContext.AppendCategoriesMetaData(response.ProductsPaged);

            return Results.Ok(response.ProductsPaged);

        })
        .Produces<PagedList<GetProductWithCatQueryResponse>>(StatusCodes.Status200OK)
        .RequireAuthorization()
        .WithTags(productsTag);

        /// <summary>
        /// Retrieves a product along with its category details based on the provided product ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve along with its category.</param>
        /// <param name="mediator">The MediatR instance responsible for processing the request.</param>
        /// <response code="200">Returns the product with its associated category details.</response>
        /// <response code="404">If the product with the specified ID is not found.</response>
        endpoints.MapGet("products/category/{id:Guid}", async ([FromRoute] Guid id,
                                                               [FromServices] IMediator mediator) => 
        {
            GetProductWithCatQueryResponse response = await mediator.Send(new GetProductWithCatQueryRequest(id));
            return Results.Ok(response);
        })
        .Produces<GetProductWithCatQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithGetByIdProductWithCategoryDoc();
        
        #endregion

        #region Post

        endpoints.MapPost("products", async ([FromBody] CreateProductCommandRequest request, 
                                             [FromServices] IMediator mediator) =>
        {
           CreateProductCommandResponse response = await mediator.Send(request);

           return Results.CreatedAtRoute(
            routeName: "GetProductById",
            routeValues: new {id = response.Id},
            value: response);

        })
        .Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithTags(productsTag);

        endpoints.MapPost("products/category-name", async ([FromBody] CreateProductByCatNameCommandRequest request,
                                                      [FromServices] IMediator mediator) =>
        {
            CreateProductCommandResponse response = await mediator.Send(request);

            return Results.CreatedAtRoute(
              routeName: "GetProductById", 
              routeValues: new { id = response.Id },
              value: response);

        })
          .Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .RequireAuthorization()
          .WithTags(productsTag);

        #endregion

        #region Put

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

  
        endpoints.MapDelete("products/{id:Guid}", async ([FromRoute] Guid id,
                                                  [FromServices] IMediator mediator) =>
        {
            DeleteProductCommandResponse response = await mediator.Send(new DeleteProductCommandRequest(id));
            return Results.Ok(response);

        })
        .Produces<DeleteProductCommandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithTags(productsTag);

        #endregion
    }
}
