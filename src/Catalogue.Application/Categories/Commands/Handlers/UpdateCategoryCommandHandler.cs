using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Catalogue.Application.Categories.Commands.Handlers;

public class UpdateCategoryCommandHandler :
    IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateCategoryCommandRequest> _validator;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork,
                                        IMapper mapper,
                                        IValidator<UpdateCategoryCommandRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request,
                                                            CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetAsNoTrackingAsync(c => c.Id == request.Id) is not Category category)
        {
            string errorMessage = string.Format(ErrorMessagesResource.NOT_FOUND_CATEGORY_MESSAGE, request.Id);
            throw new NotFoundException(errorMessage);
        }
        request.CreatedAt = category.CreatedAt;
        Validate(request);

        Category categoryToUpdate = _mapper.Map<Category>(request);

        _unitOfWork.CategoryRepository.Update(categoryToUpdate);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<UpdateCategoryCommandResponse>(categoryToUpdate);
    }

    public void Validate(UpdateCategoryCommandRequest request) 
    {
        var result = _validator.Validate(request);
        
        if(!result.IsValid) 
        {
            IList<string> errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
