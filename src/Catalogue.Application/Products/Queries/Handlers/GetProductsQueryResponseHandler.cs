using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalogue.Application.Pagination;
using Catalogue.Application.Products.Queries.Requests;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Products.Queries.Handlers;

public class GetProductsQueryResponseHandler :
    IRequestHandler<GetProductsQueryRequest, GetProductsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryResponseHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetProductsQueryResponse> Handle(GetProductsQueryRequest request, 
                                                       CancellationToken cancellationToken) 
    {
         var productsPaged = await PagedList<GetProductQueryResponse>.ToPagedListAsync(request.Parameters.PageNumber, 
                                                             request.Parameters.PageSize, 
                                                             _unitOfWork.ProductRepository.GetAll()
                                                             .ProjectTo<GetProductQueryResponse>(_mapper.ConfigurationProvider));


        return new GetProductsQueryResponse(productsPaged);
    }
}