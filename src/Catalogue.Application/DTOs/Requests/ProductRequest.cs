using Catalogue.Application.Abstractions;
using System.Text.Json.Serialization;

namespace Catalogue.Application.DTOs.Requests;

public class ProductRequest : ProductBase
{
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public Guid CategoryId { get; set; }
}
