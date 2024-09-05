using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.FluentValidation;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Catalogue.Application.Categories.Commands.Handlers;

public sealed class CreateCategoryCommandHandler :
    IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCategoryCommandRequest> _validator;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, 
                                        IMapper mapper, 
                                        IValidator<CreateCategoryCommandRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request,
                                                            CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetAsNoTrackingAsync(c => c.Name.ToLower() == request.Name.ToLower()) is not null)
        {
            string existsMessage = string.Format(ErrorMessagesResource.NAME_EXISTS_MESSAGE, request.Name);
            throw new ExistsValueException(existsMessage);
        }

        _validator.EnsureValid(request);

        var categoryToAdd = _mapper.Map<Category>(request);

        await _unitOfWork.CategoryRepository.AddAsync(categoryToAdd);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CreateCategoryCommandResponse>(categoryToAdd);
    }
}
