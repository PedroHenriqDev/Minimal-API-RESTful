using Catalogue.Application.Abstractions.DTOs;
using System.Text.Json.Serialization;

namespace Catalogue.Application.DTOs.Requests;

public class ProductRequest : ProductBase
{
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public int CategoryId { get; set; }
}
