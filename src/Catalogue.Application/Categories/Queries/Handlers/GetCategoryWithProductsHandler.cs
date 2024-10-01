using AutoMapper;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Handlers;

public sealed class GetCategoryWithProductsHandler
    : IRequestHandler<GetCategoryWithProdsQueryRequest, GetCategoryWithProdsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryWithProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCategoryWithProdsQueryResponse> Handle(GetCategoryWithProdsQueryRequest request,
                                       CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(request.Id) is Category category)
        {
            return _mapper.Map<GetCategoryWithProdsQueryResponse>(category);
        }

        string errorMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Category).Name, request.Id);
        throw new NotFoundException(errorMessage);
    }
}
