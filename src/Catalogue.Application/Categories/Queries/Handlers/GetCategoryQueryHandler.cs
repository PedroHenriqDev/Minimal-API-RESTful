using AutoMapper;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Resources;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Handlers;

public sealed class GetCategoryQueryHandler
    : IRequestHandler<GetCategoryQueryRequest, GetCategoryQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public GetCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCategoryQueryResponse> Handle(GetCategoryQueryRequest request,
                                                       CancellationToken cancellationToken)
    {
        if (await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id) is Category category)
        {
            return _mapper.Map<GetCategoryQueryResponse>(category);
        }

        string errorMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Category).Name, request.Id);
        throw new NotFoundException(errorMessage);
    }
}
