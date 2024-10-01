using AutoMapper;
using Catalogue.Application.Categories.Queries.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Interfaces.Services;
using Catalogue.Application.Resources;
using Catalogue.Application.Services;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Handlers;

public sealed class GetCategoryStatisticsHandler :
    IRequestHandler<GetCategoryStatisticsQueryRequest, GetCategoryStatisticsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private IStatisticsService _statsService;

    public GetCategoryStatisticsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCategoryStatisticsQueryResponse> Handle(GetCategoryStatisticsQueryRequest request,
                                                                   CancellationToken cancellationToken)
    {
        if(await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(request.Id) is Category category)
        {
            _statsService = new StatisticsService(category.Products.Select(p => p.Price));

            GetCategoryWithProdsQueryResponse categoryResponse =
                _mapper.Map<GetCategoryWithProdsQueryResponse>(category);

            var statsResponse = new StatsResponse()
            {
                Average = _statsService.Average(),
                Mode = _statsService.Mode(),
                Variance = _statsService.Variance(),
                StandartDeviation = _statsService.StandartDeviation(),
                Quantity = _statsService.Quantity
            };

            return new GetCategoryStatisticsQueryResponse(categoryResponse, statsResponse);
        }

        string notFoundMessage =
            string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(Category).Name, request.Id);

        throw new NotFoundException(notFoundMessage);
    }
}