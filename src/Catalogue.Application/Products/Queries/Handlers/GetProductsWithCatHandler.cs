using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalogue.Application.Pagination;
using Catalogue.Application.Products.Queries.Requests;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Application.Products.Queries.Handlers;

public class GetProductsWithCatHandler : 
        IRequestHandler<GetProductsWithCatQueryRequest, GetProductsWithCatQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsWithCatHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetProductsWithCatQueryResponse> Handle(GetProductsWithCatQueryRequest request,
                                                             CancellationToken cancellationToken) 
    {
        var productsPaged = await PagedList<GetProductWithCatQueryResponse>
            .ToPagedListAsync(request.Parameters.PageNumber,
                              request.Parameters.PageSize,
                              _unitOfWork.ProductRepository.GetAll()
                              .Include(p => p.Category)
                              .ProjectTo<GetProductWithCatQueryResponse>(_mapper.ConfigurationProvider));

        return new GetProductsWithCatQueryResponse(productsPaged);
    }
}
