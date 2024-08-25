using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Pagination;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Handlers;

public class GetCategoriesWithProductsHandler : IRequestHandler<GetCategoriesWithProductsQueryRequest, GetCategoriesWithProductsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesWithProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCategoriesWithProductsQueryResponse> Handle(GetCategoriesWithProductsQueryRequest request,
                                                                     CancellationToken cancellationToken) 
    {
        IQueryable<Category>? categories = _unitOfWork.CategoryRepository.GetAllWithProducts();

        var categoriesPaged = await PagedList<GetCategoryWithProductsQueryResponse>
            .ToPagedListAsync(request.Parameters.PageNumber,
                              request.Parameters.PageSize,
                              categories.ProjectTo<GetCategoryWithProductsQueryResponse>(_mapper.ConfigurationProvider));

        return new GetCategoriesWithProductsQueryResponse(categoriesPaged);
    }
}
