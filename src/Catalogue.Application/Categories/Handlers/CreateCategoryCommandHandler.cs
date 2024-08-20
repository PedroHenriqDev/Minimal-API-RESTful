using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Response;
using Catalogue.Application.Categories.Commands.Validations;
using Catalogue.Application.Exceptions;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Catalogue.Application.Categories.Handlers;

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
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
        Validate(request);
        var categoryToAdd = _mapper.Map<Category>(request);
        Category categoryAdded = await _unitOfWork.CategoryRepository.AddAsync(categoryToAdd);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<CreateCategoryCommandResponse>(categoryAdded);
    }

    public void Validate(CreateCategoryCommandRequest request) 
    {
        var result = _validator.Validate(request);
        
        if(!result.IsValid) 
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages); 
        }
    }
}
