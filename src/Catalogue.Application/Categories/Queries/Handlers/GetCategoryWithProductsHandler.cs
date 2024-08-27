using AutoMapper;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;
using System.Text.Json;

namespace Catalogue.Application.Categories.Queries.Handlers;

public class GetCategoryWithProductsHandler
    : IRequestHandler<GetCategoryWithProductsQueryRequest, GetCategoryWithProductsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryWithProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCategoryWithProductsQueryResponse> Handle(GetCategoryWithProductsQueryRequest request,
                                       CancellationToken cancellationToken)
    {
        if(await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(request.Id) is not Category category) 
        {
            var errorMessage = string.Format(ErrorMessagesResource.NOT_FOUND_CATEGORY_MESSAGE, request.Id);
            throw new NotFoundException(errorMessage);
        }

        return _mapper.Map<GetCategoryWithProductsQueryResponse>(category);
    }
}
