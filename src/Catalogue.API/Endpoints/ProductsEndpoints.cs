using Catalogue.Application.DTOs;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class ProductsEndpoints
{
    const string endpointsTag = "Products";

    public static void MapPostProductsEndpoints(this WebApplication app) 
    {
        app.MapPost("products", async ([FromBody] CreateProductCommandRequest request,
                                       [FromServices] IMediator mediator) =>
        {
            CreateProductCommandResponse response = await mediator.Send(request);

            return Results.Created(string.Empty, response);

        }).Produces<CreateProductCommandResponse>(StatusCodes.Status201Created)
          .Produces<ErrorsDto>(StatusCodes.Status400BadRequest)
          .Produces<ErrorsDto>(StatusCodes.Status404NotFound)
          .WithTags(endpointsTag);
    }
}
