using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class CategoriesEndpoints
{
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
        app.MapDelete("Categories/{id:int}", async ([FromBody] DeleteCategoryCommandRequest request,
                                                    [FromRoute] int id,
                                                    [FromServices] IMediator mediator) =>
        {
            request.Id = id;
            DeleteCategoryCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);
        }).Produces<DeleteCategoryCommandResponse>(StatusCodes.Status200OK)
          .Produces<ErrorsDto>(StatusCodes.Status404NotFound);
    }

    public static void MapUpdateCategoryEndpoints(this WebApplication app)
    {
        app.MapPut("Categories/{id:int}", async ([FromBody] UpdateCategoryCommandRequest request,
                                                 [FromRoute] int id,
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
