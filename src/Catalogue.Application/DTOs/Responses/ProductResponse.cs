using Catalogue.Application.Abstractions;
using System.Text.Json.Serialization;

namespace Catalogue.Application.DTOs.Responses;

public sealed class ProductResponse : ProductBase
{
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public int CategoryId { get; set; }
}
