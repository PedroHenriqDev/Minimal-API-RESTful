using Catalogue.API.Filters;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class ProductsEndpoints
{
    const string endpointTag = "Products";

    public static void MapPostProductsEndpoints(this WebApplication app) 
    {
        app.MapPost("products", async ([FromBody] CreateProductCommandRequest request, 
                                       [FromServices] IMediator mediator) =>
        {
            CreateProductCommandResponse response = await mediator.Send(request);

            return Results.Created(string.Empty, response);
        }).Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);
        
        app.MapPost("products/category-name", async ([FromBody] CreateProductByCatNameCommandRequest request,
                                       [FromServices] IMediator mediator) =>
        {
            CreateProductCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);

        }).Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);
    }

    public static void MapPutProductsEndpoints(this WebApplication app) 
    {
        app.MapPut("products/{id:int}", async ([FromBody] UpdateProductCommandRequest request,
                                               [FromRoute] int id,
                                               [FromServices] IMediator mediator) =>
        {
            UpdateProductCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);

        })
          .AddEndpointFilter<InjectIdFilter>()
          .Produces<UpdateProductCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);                    
    }

    public static void MapDeleteProductsEndpoints(this WebApplication app) 
    {
        app.MapDelete("products/{id:int}", async ([FromRoute] int id,
                                                  [FromServices] IMediator mediator) =>
        {
            DeleteProductCommandResponse response = await mediator.Send(new DeleteProductCommandRequest(id));
            return Results.Ok(response);

        }).Produces<DeleteProductCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
          .WithTags(endpointTag);
    }
}
