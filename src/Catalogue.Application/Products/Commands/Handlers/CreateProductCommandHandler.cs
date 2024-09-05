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

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
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
        if(await _unitOfWork.CategoryRepository.GetAsNoTrackingAsync(c => c.Id == request.CategoryId) is Category) 
        {
            _validator.EnsureValid(request);

            var productToAdd = _mapper.Map<Product>(request);

            await _unitOfWork.ProductRepository.AddAsync(productToAdd);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CreateProductCommandResponse>(productToAdd);
        }

        string notFoundMessage =
            string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Category).Name, request.CategoryId);

        throw new NotFoundException(notFoundMessage); 
    }
}
