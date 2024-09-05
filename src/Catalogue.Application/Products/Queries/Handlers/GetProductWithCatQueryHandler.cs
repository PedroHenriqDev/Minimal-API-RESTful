using AutoMapper;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Products.Queries.Requests;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Products.Queries.Handlers;

public sealed class GetProductWithCatQueryHandler : 
    IRequestHandler<GetProductWithCatQueryRequest, GetProductWithCatQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductWithCatQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetProductWithCatQueryResponse> Handle(GetProductWithCatQueryRequest request,
                                                             CancellationToken cancellationToken)
    {
        if(await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(request.Id) is Product product)
        {
            return _mapper.Map<GetProductWithCatQueryResponse>(product);
        }

        string notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Product).Name, request.Id);
        throw new NotFoundException(notFoundMessage);
    }
}
