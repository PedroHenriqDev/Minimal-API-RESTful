using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Categories.Commands.Handlers;

public class DeleteCategoryCommandHandler :
    IRequestHandler<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork,
                                        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommandRequest request,
                                                            CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetAsync(c => c.Id == request.Id) is Category category)
        {
            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<DeleteCategoryCommandResponse>(category);
        }

        string errorMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Category).Name, request.Id);
        throw new NotFoundException(errorMessage);
    }
}
