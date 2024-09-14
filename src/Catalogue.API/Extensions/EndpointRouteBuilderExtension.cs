using Asp.Versioning;

namespace Catalogue.API.Extensions;

public static class EndpointRouteBuilderExtension
{
    public static IEndpointRouteBuilder ConfigureEndpointsVersioning(this IEndpointRouteBuilder endpoints, ApiVersion version)
    {
        endpoints.NewApiVersionSet().HasApiVersion(version);
        return endpoints;
    }
} 