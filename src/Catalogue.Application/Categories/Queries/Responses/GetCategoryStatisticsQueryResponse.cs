using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Categories.Queries.Responses;
                            
public sealed class GetCategoryStatisticsQueryResponse
{
    public GetCategoryWithProdsQueryResponse Category { get; set; }
    public StatsResponse Stats { get; set; }

    public GetCategoryStatisticsQueryResponse(GetCategoryWithProdsQueryResponse category, StatsResponse stats)
    {
        Category = category;
        Stats = stats;
    }

    public GetCategoryStatisticsQueryResponse()
    {
    }
}