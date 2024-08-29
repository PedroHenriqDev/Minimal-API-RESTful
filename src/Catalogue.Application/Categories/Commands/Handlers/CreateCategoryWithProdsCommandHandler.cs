using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.DTOs.Requests;
using Catalogue.Application.Exceptions;
using Catalogue.Application.FluentValidation;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Catalogue.Application.Categories.Commands.Handlers;

public class CreateCategoryWithProdsCommandHandler :
    IRequestHandler<CreateCategoryWithProdsCommandRequest, CreateCategoryWithProdsCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCategoryWithProdsCommandRequest> _validator;

    public CreateCategoryWithProdsCommandHandler(IUnitOfWork unitOfWork,
                                                 IMapper mapper,
                                                 IValidator<CreateCategoryWithProdsCommandRequest> validator)
                                                
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CreateCategoryWithProdsCommandResponse> Handle(CreateCategoryWithProdsCommandRequest request,
                                                                     CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetAsNoTrackingAsync(c =>
            c.Name.ToLower() == request.Name.ToLower()) is not null)
        {
            string existsMessage = string.Format(ErrorMessagesResource.NAME_EXISTS_MESSAGE, request.Name);
            throw new ExistsValueException(existsMessage);
        }

        _validator.EnsureValid(request);

        var categoryToAdd = _mapper.Map<Category>(request);

        await _unitOfWork.CategoryRepository.AddAsync(categoryToAdd);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CreateCategoryWithProdsCommandResponse>(categoryToAdd);
    }
}
