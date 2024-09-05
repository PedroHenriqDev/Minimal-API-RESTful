using AutoMapper;
using Catalogue.Application.Exceptions;
using Catalogue.Application.FluentValidation;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Catalogue.Application.Products.Commands.Handlers;

public sealed class CreateProductByCatNameCommandHandler : 
    IRequestHandler<CreateProductByCatNameCommandRequest, CreateProductCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductByCatNameCommandRequest> _validator;

    public CreateProductByCatNameCommandHandler(IUnitOfWork unitOfWork,
                                       IMapper mapper, 
                                       IValidator<CreateProductByCatNameCommandRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductByCatNameCommandRequest request, 
                                                           CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetAsNoTrackingAsync(c => 
            c.Name.ToLower() == request.CategoryName.ToLower()) is Category category) 
        {
            _validator.EnsureValid(request);

            request.CategoryId = category.Id;

            var productToAdd = _mapper.Map<Product>(request);

            await _unitOfWork.ProductRepository.AddAsync(productToAdd);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CreateProductCommandResponse>(productToAdd);
        }

        string notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_CATEGORY_NAME, request.CategoryName);
        throw new NotFoundException(notFoundMessage);
    }
}
