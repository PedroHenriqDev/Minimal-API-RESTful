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

public sealed class UpdateProductCommandHandler :
    IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateProductCommandRequest> _validator;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<UpdateProductCommandRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, 
                                                           CancellationToken cancellationToken)
    {
        if(await _unitOfWork.ProductRepository.GetAsync(p => p.Id == request.Id) is Product productToUpdate) 
        {
            _validator.EnsureValid(request);

            _mapper.Map(request, productToUpdate);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UpdateProductCommandResponse>(productToUpdate);
        }

        string notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Product).Name, request.Id);
        throw new NotFoundException(notFoundMessage);
    }
}
