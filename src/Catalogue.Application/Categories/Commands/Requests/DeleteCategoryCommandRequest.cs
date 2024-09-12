using Catalogue.Application.Categories.Commands.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Commands.Requests;

public sealed class DeleteCategoryCommandRequest : IRequest<DeleteCategoryCommandResponse>
{
    public Guid Id { get; set; }

    public DeleteCategoryCommandRequest(Guid id)
    {
        Id = id;
    }
}
