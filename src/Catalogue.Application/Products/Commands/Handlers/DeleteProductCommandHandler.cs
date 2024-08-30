using AutoMapper;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Products.Commands.Handlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken) 
    {
        if (await _unitOfWork.ProductRepository.GetAsync(p => p.Id == request.Id) is Product product)
        {
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.CommitAsync();
            
            return _mapper.Map<DeleteProductCommandResponse>(product);
        }

        string notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Product).Name, request.Id);
        throw new NotFoundException(notFoundMessage);
    }
}
