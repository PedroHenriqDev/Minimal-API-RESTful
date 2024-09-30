using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Categories.Queries.Responses;
                            
public class GetCategoryStatisticsCommandResponse
{
    public GetCategoryWithProdsQueryResponse Category { get; set; }
    public StatsResponse Stats { get; set; }

    public GetCategoryStatisticsCommandResponse(GetCategoryWithProdsQueryResponse category, StatsResponse stats)
    {
        Category = category;
        Stats = stats;
    }

    public GetCategoryStatisticsCommandResponse()
    {
    }
}