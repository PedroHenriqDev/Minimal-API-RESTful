using Asp.Versioning;

namespace Catalogue.API.OpenApi;

public static class EndpointRouteBuilderExtension
{
    public static IEndpointRouteBuilder ConfigureEndpointsVersioning(this IEndpointRouteBuilder endpoints, ApiVersion version)
    {
        endpoints.NewApiVersionSet().HasApiVersion(version);
        return endpoints;
    }
} 