using Catalogue.Application.Categories.Commands.Abstractions;
using Catalogue.Application.Categories.Commands.Response;
using MediatR;

namespace Catalogue.Application.Categories.Commands.Requests;

public class CreateCategoryCommandRequest : CategoryCommandBase, IRequest<CreateCategoryCommandResponse>
{
}
