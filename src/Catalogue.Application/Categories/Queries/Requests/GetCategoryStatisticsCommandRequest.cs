using System.Text.Json.Serialization;
using Catalogue.Application.Categories.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoryStatisticsCommandRequest : IRequest<GetCategoryStatisticsCommandResponse>
{
    [JsonIgnore]
    public Guid Id {get; set;}

    public GetCategoryStatisticsCommandRequest(Guid id)
        => Id = id;
}