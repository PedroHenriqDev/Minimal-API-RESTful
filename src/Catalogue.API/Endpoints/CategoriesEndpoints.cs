using Catalogue.API.OpenApi;
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
    private const string categoriesTag = "Categories";

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

    /// <summary>
    /// The endpoint to retrieve a list of categories with pagination.
    /// </summary>
    /// <param name="httpContext">The HTTP context of the request.</param>
    /// <param name="parameters">The query parameters defining pagination (page number and page size).</param>
    /// <param name="mediator">The instance of the Mediator to send the request.</param>
    /// <returns>A result containing a paginated list of categories.</returns>
    /// <remarks>
    /// The pagination metadata includes details such as page size, current page, and total item count.
    /// </remarks>
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

    /// <summary>
    /// Retrieves a category by its unique identifier (GUID).
    /// </summary>
    /// <param name="id">The unique identifier of the category (GUID).</param>
    /// <param name="mediator">The mediator used to handle the request and retrieve
    /// the category.</param>
    /// <returns>
    /// Returns an HTTP 200 OK response with the category details if found. 
    /// If the category is not found, it returns an HTTP 404 Not Found response with
    /// an error message.
    /// </returns>
    /// <remarks>
    /// This endpoint uses a `GUID` as a route parameter to identify the category.
    /// </remarks>
    private static async Task<IResult> GetCategoryByIdAsync([FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        GetCategoryQueryResponse response = await mediator.Send(new GetCategoryQueryRequest(id));
        return Results.Ok(response);
    }
    
    /// <summary>
    /// Endpoint to retrieve a paginated list of categories along with their associated products.
    /// </summary>
    /// <param name="httpContext">The HTTP context containing information about the request and
    /// response.</param>
    /// <param name="parameters">Pagination parameters, such as page number and page size.</param>
    /// <param name="mediator">The MediatR instance used to send the request and retrieve the result.</param>
    /// <returns>
    /// Returns a paginated list of categories with their associated products, along with pagination
    /// metadata in the response header.
    /// </returns>
    /// <response code="200">Returns a paginated list of categories with products.</response>
    /// <response code="400">Returns an error response if there is an issue with the request.</response>
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

    /// <summary>
    /// Retrieves a category and its associated products based on the category ID.
    /// </summary>
    /// <param name="id">The unique identifier (Guid) of the category.</param>
    /// <param name="mediator">The MediatR instance to handle the query request.</param>
    /// <returns>
    /// Returns the category along with its products if found.
    /// If the category or products are not found, it returns a 404 Not Found response.
    /// </returns>
    /// <response code="200">Category and associated products successfully retrieved.</response>
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

    /// <summary>
    /// Create a new category along with associated products.
    /// </summary>
    /// <param name="request">An object containing the necessary data to create the category and its products.</param>
    /// <param name="mediator">The MediatR instance responsible for handling the request.</param>
    /// <response code="201">Returns 2OO Ok and the created category along with associated products.</response>
    /// <response code="400">Returns 400 Bad Request if the category data is invalid.</response>
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

    /// <summary>
    /// Create a new category along with associated products.
    /// </summary>
    /// <param name="request">An object containing the necessary data to create the category and its products.</param>
    /// <param name="mediator">The MediatR instance responsible for handling the request.</param>
    /// <response code="201">Returns 2OO Ok and the created category along with associated products.</response>
    /// <response code="400">Returns 400 Bad Request if the category data is invalid.</response>
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

    /// <summary>
    /// Updates a category by its unique identifier.
    /// </summary>
    /// <param name="request">An object containing the necessary data to update the category.</param>
    /// <param name="id">The unique identifier (Guid) of the category to be updated.</param>
    /// <param name="mediator">The MediatR instance responsible for processing the update request.</param>
    /// <response code="200">Returns 200 Ok if the category data is valid and successfully updated.</response>
    /// <response code="400">Returns 400 Bad Request if the provided category data is invalid.</response>
    /// <response code="404">Returns 404 Not Found if the specified category id does not exist.</response>
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

    /// <summary>
    /// Deletes a category using its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (Guid) of the category to be deleted.</param>
    /// <param name="mediator">The MediatR instance responsible for processing the deletion request.</param>
    /// <response code="200">Returns 200 OK if the category ID is valid and the deletion is successful.</response>
    /// <response code="404">Returns 404 Not Found if the specified category ID does not exist.</response>
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

