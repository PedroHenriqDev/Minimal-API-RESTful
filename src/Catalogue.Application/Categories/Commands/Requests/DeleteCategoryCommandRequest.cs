using Catalogue.Application.Categories.Commands.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Commands.Requests;

public class DeleteCategoryCommandRequest : IRequest<DeleteCategoryCommandResponse>
{
    public int Id { get; set; }

    public DeleteCategoryCommandRequest(int id)
    {
        Id = id;
    }
}
