using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Pagination;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Handlers;

public sealed class GetCategoriesWithProductsHandler
    : IRequestHandler<GetCategoriesWithProdsQueryRequest, GetCategoriesWithProdsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesWithProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCategoriesWithProdsQueryResponse> Handle(GetCategoriesWithProdsQueryRequest request,
                                                                     CancellationToken cancellationToken) 
    {
        IQueryable<Category>? categories = await _unitOfWork.CategoryRepository.GetWithProductsAsync();

        var categoriesPaged = await PagedList<GetCategoryWithProdsQueryResponse>
            .ToPagedListAsync(request.Parameters.PageNumber,
                              request.Parameters.PageSize,
                              categories.ProjectTo<GetCategoryWithProdsQueryResponse>(_mapper.ConfigurationProvider));

        return new GetCategoriesWithProdsQueryResponse(categoriesPaged);
    }
}
