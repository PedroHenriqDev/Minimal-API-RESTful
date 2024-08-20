using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class CategoriesEndpoints
{
    public static void MapPostCategory(this WebApplication app) 
    {
        app.MapPost("Categories", async ([FromBody] CreateCategoryCommandRequest request,
                                         [FromServices] IMediator mediator) =>
        {
            CreateCategoryCommandResponse newCategory = await mediator.Send(request);

            return Results.Created(string.Empty, newCategory);
        });
    }
}
