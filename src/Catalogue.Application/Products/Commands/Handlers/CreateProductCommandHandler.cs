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

public class CreateProductCommandHandler : 
    IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductCommandRequest> _validator;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork,
                                       IMapper mapper, 
                                       IValidator<CreateProductCommandRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, 
                                                           CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetAsync(c => c.Name.ToLower() == request.CategoryName.ToLower())
            is Category category) 
        {
            _validator.EnsureValid(request);

            request.CategoryId = category.Id;

            var product = _mapper.Map<Product>(request);

            Product productCreated = await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CreateProductCommandResponse>(productCreated);
        }

        string notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_CATEGORY_NAME, request.CategoryName);
        throw new NotFoundException(notFoundMessage);
    }
}
