using AutoMapper;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Products.Queries.Requests;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Products.Queries.Handlers;

public sealed class GetProductQueryHandler : IRequestHandler<GetProductQueryRequest, GetProductQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetProductQueryResponse> Handle(GetProductQueryRequest request, 
                                                      CancellationToken cancellationToken)
    {
        if(await _unitOfWork.ProductRepository.GetAsync(p => p.Id == request.Id) is Product product) 
        {
            return _mapper.Map<GetProductQueryResponse>(product);
        }

        string notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Product).Name, request.Id);
        throw new NotFoundException(notFoundMessage);
    }
}
