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

        /// <summary>
        /// The endpoint to retrieve a list of categories with pagination.
        /// </summary>
        /// <param name="httpContext">The HTTP context of the request.</param>
        /// <param name="parameters">The query parameters defining pagination (page number and page size).</param>
        /// <param name="mediator">The instance of the Mediator to send the request.</param>
        /// <returns>A result containing a paginated list of categories.</returns>
        /// <remarks>
        /// The pagination metadata includes details such as page size, current page,
        /// and total item count.
        /// </remarks>
        endpoints.MapGet("categories", async (HttpContext httpContext,
                                              [AsParameters] QueryParameters parameters,
                                              [FromServices] IMediator mediator) =>
        {
            GetCategoriesQueryResponse response =
                        await mediator.Send(new GetCategoriesQueryRequest(parameters));

            httpContext.AppendCategoriesMetaData(response.CategoriesPaged);

            return Results.Ok(response.CategoriesPaged);

        })
        .Produces<PagedList<GetCategoryQueryResponse>>(StatusCodes.Status200OK)
        .WithGetCategoriesDoc();

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
        endpoints.MapGet("categories/{id:guid}", async ([FromRoute] Guid id, 
                                                       [FromServices] IMediator mediator) =>
        {
            GetCategoryQueryResponse response = await mediator.Send(new GetCategoryQueryRequest(id));
            return Results.Ok(response);

        })
        .Produces<GetCategoryQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithGetCategoryByIdDoc()
        .WithName("GetCategoryById");

        endpoints.MapGet("categories/products", async (HttpContext httpContext,
                                                       [AsParameters] QueryParameters parameters,
                                                       [FromServices] IMediator mediator) =>
        {
            GetCategoriesWithProdsQueryResponse response = 
                           await mediator.Send(new GetCategoriesWithProdsQueryRequest(parameters));
            httpContext.AppendCategoriesMetaData(response.CategoriesPaged);

            return Results.Ok(response.CategoriesPaged);

        })
        .Produces<PagedList<GetCategoryWithProdsQueryResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithTags(categoriesTag);

        endpoints.MapGet("categories/{id:Guid}/products", async ([FromRoute] Guid id,
                                                                [FromServices] IMediator mediator) =>
        {
            GetCategoryWithProdsQueryResponse response =
                           await mediator.Send(new GetCategoryWithProdsQueryRequest(id));

            return Results.Ok(response);

        })
        .Produces<GetCategoryWithProdsQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithTags(categoriesTag);
        #endregion

        #region Post
        endpoints.MapPost("categories", async ([FromBody] CreateCategoryCommandRequest request,
                                               [FromServices] IMediator mediator) =>
        {
            CreateCategoryCommandResponse response = await mediator.Send(request);
            
            return Results.CreatedAtRoute(
              routeName: "GetCategoryById",
              routeValues: new {id = response.Id},
              value: response);

        })
        .Produces<CreateCategoryCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithTags(categoriesTag);

        endpoints.MapPost("categories/products", async ([FromBody] CreateCategoryWithProdsCommandRequest request,
                                                        [FromServices] IMediator mediator) =>
        {
            CreateCategoryWithProdsCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);

        })
        .Produces<CreateCategoryWithProdsCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithTags(categoriesTag);
        #endregion

        #region Put

        endpoints.MapPut("categories/{id:Guid}", async ([FromBody] UpdateCategoryCommandRequest request,
                                                       [FromRoute] Guid id,
                                                       [FromServices] IMediator mediator) =>
        {
            UpdateCategoryCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);

        })
        .AddEndpointFilter<InjectIdFilter>()
        .Produces<UpdateCategoryCommandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithTags(categoriesTag);

        #endregion
       
        #region Delete
        endpoints.MapDelete("categories/{id:Guid}", async ([FromRoute] Guid id,
                                                          [FromServices] IMediator mediator) =>
        {
            DeleteCategoryCommandResponse response = await mediator.Send(new DeleteCategoryCommandRequest(id));
            return Results.Ok(response);
        })
        .Produces<DeleteCategoryCommandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .WithTags(categoriesTag);
        #endregion
    }
}

